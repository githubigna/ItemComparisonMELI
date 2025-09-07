using System.Text.Json;
using ItemComparison.Api.Models;

namespace ItemComparison.Api.Data;

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products;

    public InMemoryProductRepository(IWebHostEnvironment env)
    {
        var path = Path.Combine(env.ContentRootPath, "products.json");
        if (!File.Exists(path))
            _products = new();
        else
        {
            var json = File.ReadAllText(path);
            _products = JsonSerializer.Deserialize<List<Product>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
    }

    public Task<IReadOnlyList<Product>> GetAllAsync(string? q, int page, int pageSize)
    {
        IEnumerable<Product> query = _products;

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim().ToLowerInvariant();
            query = query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                p.Description.ToLower().Contains(term) ||
                p.Specs.Any(s => s.Key.ToLower().Contains(term) || s.Value.ToLower().Contains(term)));
        }

        query = query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return Task.FromResult((IReadOnlyList<Product>)query.ToList());
    }

    public Task<Product?> GetByIdAsync(string id) =>
        Task.FromResult(_products.FirstOrDefault(p => p.Id.Equals(id, StringComparison.OrdinalIgnoreCase)));

    public Task<IReadOnlyList<Product>> GetManyAsync(IEnumerable<string> ids)
    {
        var set = ids.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var list = _products.Where(p => set.Contains(p.Id)).ToList();
        return Task.FromResult((IReadOnlyList<Product>)list);
    }
}
