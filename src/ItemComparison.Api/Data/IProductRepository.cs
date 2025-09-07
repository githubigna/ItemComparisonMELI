using ItemComparison.Api.Models;

namespace ItemComparison.Api.Data;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(string? q = null, int page = 1, int pageSize = 20);
    Task<Product?> GetByIdAsync(string id);
    Task<IReadOnlyList<Product>> GetManyAsync(IEnumerable<string> ids);
}
