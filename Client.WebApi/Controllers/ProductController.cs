using Application.Common.ResponseWrapper;
using Application.Common.Pagination;
using Application.Services.Product;
using Application.Services.Product.Request;
using Application.Services.Product.Request.CreateProduct;
using Application.Services.Product.Request.UpdateProduct;
using Application.Services.Product.Response;
using Domain.Product;
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
    public async Task<ActionResult<ApiResponse<List<GetProductsResponse>>>> GetProducts([FromQuery] GetProductsQuery query)
    {
        var products = await _productService.GetProductsAsync(query);
        var response = ApiResponse<List<GetProductsResponse>>.SuccessWithMetaData(products.Data, products.Metadata, "Get all products successfully");

        return Ok(response);
    }

    // READ BY ID
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductEntity>>> GetProduct(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        return Ok(ApiResponse<ProductEntity>.Success(product, "Get product successfully."));
    }

    // CREATE
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductEntity>>> CreateProduct([FromBody] CreateProductRequest request)
    {
        var product = await _productService.CreateProductAsync(request);
        return Ok(ApiResponse<ProductEntity>.Success(product, "Product created successfully."));
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProductEntity>>> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
    {
        var product = await _productService.UpdateProductAsync(id, request);

        return Ok(ApiResponse<ProductEntity>.Success(product, "Product updated successfully."));
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteProduct(int id)
    {
        await _productService.DeleteProductAsync(id);

        return Ok(ApiResponse<string>.Success(null, "Product deleted successfully."));
    }
}
