using ItemComparison.Api.Data;
using ItemComparison.Api.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using ItemComparison.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CompareRequestValidator>();
// Health checks
builder.Services.AddHealthChecks();

// Repo en memoria leyendo products.json
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// /health (200 OK si la app está viva)
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }
