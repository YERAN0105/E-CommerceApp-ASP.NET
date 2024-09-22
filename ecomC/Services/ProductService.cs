using ecomC.DTOs;
using ecomC.Models;
using ecomC.Repositories;

namespace ecomC.Services;

// Services/ProductService.cs
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProducts() => await _productRepository.GetAllProducts();

    public async Task<Product> GetProductById(string productId) => await _productRepository.GetProductById(productId);

    public async Task CreateProduct(ProductDTO productDTO, string vendorId)
    {
        var product = new Product
        {
            Name = productDTO.Name,
            Description = productDTO.Description,
            Price = productDTO.Price,
            VendorId = vendorId,
            IsActive = true
        };
        await _productRepository.CreateProduct(product);
    }

    public async Task UpdateProduct(string productId, ProductDTO productDTO)
    {
        var product = await _productRepository.GetProductById(productId);
        if (product != null)
        {
            product.Name = productDTO.Name;
            product.Description = productDTO.Description;
            product.Price = productDTO.Price;
            await _productRepository.UpdateProduct(product);
        }
    }

    public async Task DeleteProduct(string productId) => await _productRepository.DeleteProduct(productId);

    public async Task ActivateProduct(string productId) => await _productRepository.ActivateProduct(productId);

    public async Task DeactivateProduct(string productId) => await _productRepository.DeactivateProduct(productId);
}