namespace ItemComparison.Api.Dtos;

public sealed record ProductDto(
    string Id,
    string Name,
    string Description,
    string ImageUrl,
    decimal PriceMin,
    decimal PriceMax,
    IReadOnlyList<SpecDto> Specs);

public sealed record SpecDto(string Key, string Value);
