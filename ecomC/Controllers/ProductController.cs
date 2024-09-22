using ecomC.DTOs;
using ecomC.Services;

namespace ecomC.Controllers;

// Controllers/ProductController.cs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        var product = await _productService.GetProductById(id)
            ;
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDTO)
    {
        var vendorId = User.Identity.Name; // Assuming authentication provides vendor ID
        await _productService.CreateProduct(productDTO, vendorId);
        return CreatedAtAction(nameof(GetProductById), new { id = productDTO.Name }, productDTO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductDTO productDTO)
    {
        await _productService.UpdateProduct(id, productDTO);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        await _productService.DeleteProduct(id)
            ;
        return NoContent();
    }

    [HttpPost("{id}/activate")]
    public async Task<IActionResult> ActivateProduct(string id)
    {
        await _productService.ActivateProduct(id)
            ;
        return NoContent();
    }

    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> DeactivateProduct(string id)
    {
        await _productService.DeactivateProduct(id)
            ;
        return NoContent();
    }
}