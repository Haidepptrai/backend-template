using Application.Common.Pagination;
using Application.Services.Product;
using Application.Services.Product.Request;
using Application.Services.Product.Request.CreateProduct;
using Application.Services.Product.Request.UpdateProduct;
using Application.Services.Product.Response;
using Microsoft.AspNetCore.Mvc;

namespace Client.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // READ ALL
    [HttpGet]
    public async Task<ActionResult<PagedResult<GetProductsResponse>>> GetProducts([FromQuery] GetProductsQuery query)
    {
        try
        {
            var products = await _productService.GetProductsAsync(query);
            return Ok(products);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    // READ BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    // CREATE
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        try
        {
            var product = await _productService.CreateProductAsync(request);
            return Ok(product);
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
    {
        try
        {
            var product = await _productService.UpdateProductAsync(id, request);

            if (product == null)
                return NotFound();

            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var deleted = await _productService.DeleteProductAsync(id);

            if (!deleted)
                return NotFound();

            return Ok("Product deleted successfully.");
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}
