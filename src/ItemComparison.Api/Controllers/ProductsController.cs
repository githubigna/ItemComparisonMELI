using ItemComparison.Api.Data;
using ItemComparison.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ItemComparison.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController(IProductRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(
        [FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (page <= 0 || pageSize is <= 0 or > 200) return BadRequest("Invalid paging.");
        var items = await repo.GetAllAsync(q, page, pageSize);
        return Ok(items.Select(Map));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(string id)
    {
        var item = await repo.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(Map(item));
    }

    [HttpPost("compare")]
    public async Task<ActionResult<CompareResponse>> Compare([FromBody] CompareRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var products = await repo.GetManyAsync(request.ProductIds);
        if (products.Count == 0) return NotFound("No products found.");

        var allKeys = products
            .SelectMany(p => p.Specs.Select(s => s.Key))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(k => k)
            .ToList();

        var dtos = products.Select(Map).ToList();
        return Ok(new CompareResponse(dtos, allKeys));
    }


    private static ProductDto Map(ItemComparison.Api.Models.Product p) =>
        new(p.Id, p.Name, p.Description, p.ImageUrl, p.PriceMin, p.PriceMax,
            p.Specs.Select(s => new SpecDto(s.Key, s.Value)).ToList());
}
