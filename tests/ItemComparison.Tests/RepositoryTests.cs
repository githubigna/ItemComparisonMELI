using ItemComparison.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Xunit;

public class RepositoryTests
{
    [Fact]
    public async Task GetById_Returns_Product()
    {
        var env = new HostingEnvironment { ContentRootPath = Path.GetFullPath("../../../../src/ItemComparison.Api") };
        var repo = new InMemoryProductRepository(env);
        var p = await repo.GetByIdAsync("p-iphone15");
        Assert.NotNull(p);
        Assert.Equal("iPhone 15", p!.Name);
    }

    private sealed class HostingEnvironment : IWebHostEnvironment
    {
        public string EnvironmentName { get; set; } = Environments.Development;
        public string ApplicationName { get; set; } = "ItemComparison.Api";
        public string WebRootPath { get; set; } = "";
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string ContentRootPath { get; set; } = "";
        public IFileProvider ContentRootFileProvider { get; set; } = new PhysicalFileProvider(Directory.GetCurrentDirectory());
    }
}
