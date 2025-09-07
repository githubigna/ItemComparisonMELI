namespace ItemComparison.Api.Models;

public sealed class Product
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
    public decimal PriceMin { get; set; }
    public decimal PriceMax { get; set; }
    public List<Specification> Specs { get; set; } = new();
}
