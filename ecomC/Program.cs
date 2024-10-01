using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ecomC.Configurations;
using ecomC.Repositories;
using ecomC.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(); // Required for controller mapping

// MongoDB Configuration
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<IMongoClient>(s =>
{
    var mongoSettings = s.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(mongoSettings.ConnectionString);
});
builder.Services.AddScoped(s =>
{
    var mongoSettings = s.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    var client = s.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoSettings.DatabaseName);
});

// Register IProductRepository and ProductService
builder.Services.AddScoped<IProductRepository, ProductRepository>(); // Register the repository
builder.Services.AddScoped<ProductService>(); // Register ProductService

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Configure Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("Vendor", policy => policy.RequireRole("Vendor"));
    options.AddPolicy("CSR", policy => policy.RequireRole("CSR"));
});

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Add JWT token support in Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token with 'Bearer' prefix (e.g. 'Bearer {token}')",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1"));
}

app.UseHttpsRedirection();

// Ensure that authentication and authorization middleware are in the correct order
app.UseAuthentication(); // This should come before UseAuthorization
app.UseAuthorization();

// Map controllers
app.MapControllers(); // This will map the controllers for Swagger

app.Run();
