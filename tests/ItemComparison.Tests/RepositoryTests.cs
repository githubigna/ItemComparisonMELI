using ItemComparison.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace ItemComparison.Repository.Tests;
public class RepositoryTests
{
    [Fact]
    public async Task GetById_Returns_Product()
    {
        // Busca hacia arriba hasta encontrar src/ItemComparison.Api/products.json
        var apiRoot = FindApiContentRootOrThrow();

        var env = new HostingEnvironment
        {
            ContentRootPath = apiRoot,
            ContentRootFileProvider = new PhysicalFileProvider(apiRoot),
            EnvironmentName = Environments.Development,
            ApplicationName = "ItemComparison.Api"
        };

        var repo = new InMemoryProductRepository(env);

        var p = await repo.GetByIdAsync("p-iphone15");
        Assert.NotNull(p);
        Assert.Equal("iPhone 15", p!.Name);
    }

    private static string FindApiContentRootOrThrow()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            var apiDir = Path.Combine(dir.FullName, "src", "ItemComparison.Api");
            var json = Path.Combine(apiDir, "products.json");
            if (File.Exists(json))
                return apiDir;
            dir = dir.Parent;
        }
        throw new DirectoryNotFoundException("No pude encontrar 'src/ItemComparison.Api/products.json' caminando hacia arriba desde " + AppContext.BaseDirectory);
    }

    private sealed class HostingEnvironment : IWebHostEnvironment
    {
        public string EnvironmentName { get; set; } = Environments.Development;
        public string ApplicationName { get; set; } = "";
        public string WebRootPath { get; set; } = "";
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string ContentRootPath { get; set; } = "";
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
