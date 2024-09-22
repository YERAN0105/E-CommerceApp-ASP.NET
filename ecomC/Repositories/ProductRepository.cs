using ecomC.Models;

namespace ecomC.Repositories;

// Repositories/ProductRepository.cs
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _products;

    public ProductRepository(IMongoDatabase mongoDatabase)
    {
        _products = mongoDatabase.GetCollection<Product>("Products");
    }

    public async Task<IEnumerable<Product>> GetAllProducts() => await _products.Find(p => true).ToListAsync();

    public async Task<Product> GetProductById(string productId) => 
        await _products.Find(p => p.ProductId == productId).FirstOrDefaultAsync();

    public async Task CreateProduct(Product product) => await _products.InsertOneAsync(product);

    public async Task UpdateProduct(Product product) =>
        await _products.ReplaceOneAsync(p => p.ProductId == product.ProductId, product);

    public async Task DeleteProduct(string productId) =>
        await _products.DeleteOneAsync(p => p.ProductId == productId);

    public async Task ActivateProduct(string productId)
    {
        var product = await GetProductById(productId);
        if (product != null)
        {
            product.IsActive = true;
            await UpdateProduct(product);
        }
    }

    public async Task DeactivateProduct(string productId)
    {
        var product = await GetProductById(productId);
        if (product != null)
        {
            product.IsActive = false;
            await UpdateProduct(product);
        }
    }
}