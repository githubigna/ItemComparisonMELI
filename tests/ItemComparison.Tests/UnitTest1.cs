using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ItemComparison.Api.Dtos;
using ItemComparison.Api.Validators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace ItemComparison.Tests;

public class ProductsTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    private readonly CompareRequestValidator _validator = new();

    public ProductsTests(ApiFactory factory) => _factory = factory;

    // -------- UNIT: validator --------
    [Fact]
    public void Validator_should_fail_when_ProductIds_is_null()
    {
        var req = new CompareRequest(null!);
        var result = _validator.Validate(req);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ProductIds");
    }

    [Fact]
    public void Validator_should_fail_when_ProductIds_is_empty()
    {
        var req = new CompareRequest(Array.Empty<string>());
        var result = _validator.Validate(req);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ProductIds");
    }

    [Fact]
    public void Validator_should_fail_when_ProductIds_contains_empty_values()
    {
        var req = new CompareRequest(new[] { "p-iphone15", "", " " });
        var result = _validator.Validate(req);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ProductIds");
    }

    [Fact]
    public void Validator_should_pass_with_valid_ids()
    {
        var req = new CompareRequest(new[] { "p-iphone15", "p-pixel8" });
        var result = _validator.Validate(req);
        result.IsValid.Should().BeTrue();
    }

    // -------- INTEGRATION: controller --------
    [Fact]
    public async Task Get_products_should_return_list()
    {
        var client = _factory.CreateClient();

        var resp = await client.GetAsync("/api/products");
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var products = await resp.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().NotBeNull();
        products!.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Compare_with_empty_list_should_return_400()
    {
        var client = _factory.CreateClient();

        var body = new { productIds = Array.Empty<string>() };
        var resp = await client.PostAsJsonAsync("/api/products/compare", body);

        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Compare_with_invalid_ids_should_return_404()
    {
        var client = _factory.CreateClient();

        var body = new { productIds = new[] { "no-such-id" } };
        var resp = await client.PostAsJsonAsync("/api/products/compare", body);

        resp.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Compare_with_valid_ids_should_return_200()
    {
        var client = _factory.CreateClient();

        // IDs reales desde el listado
        var list = await client.GetFromJsonAsync<List<ProductDto>>("/api/products");
        list.Should().NotBeNull();
        list!.Count.Should().BeGreaterThanOrEqualTo(2);

        var ids = list.Take(2).Select(p => p.Id).ToArray();

        var body = new { productIds = ids };
        var resp = await client.PostAsJsonAsync("/api/products/compare", body);

        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var data = await resp.Content.ReadFromJsonAsync<CompareResponse>();
        data.Should().NotBeNull();
        data!.Products.Should().NotBeEmpty();
        data.AllSpecKeys.Should().NotBeEmpty();
    }
}

// Factory para fijar el ContentRoot al proyecto de la API (encuentra products.json)
public sealed class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSolutionRelativeContentRoot("src/ItemComparison.Api");
    }
}
