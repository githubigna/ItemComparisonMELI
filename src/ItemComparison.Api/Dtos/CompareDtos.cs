namespace ItemComparison.Api.Dtos;

public sealed record CompareRequest(IReadOnlyList<string> ProductIds);

public sealed record CompareResponse(
    IReadOnlyList<ProductDto> Products,
    IReadOnlyList<string> AllSpecKeys);
