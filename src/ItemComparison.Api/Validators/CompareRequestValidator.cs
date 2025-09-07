using FluentValidation;
using ItemComparison.Api.Dtos;

namespace ItemComparison.Api.Validators;

public sealed class CompareRequestValidator : AbstractValidator<CompareRequest>
{
    public CompareRequestValidator()
    {
        RuleFor(x => x.ProductIds)
            .Cascade(CascadeMode.Stop) 
            .NotNull().WithMessage("ProductIds is required.")
            .NotEmpty().WithMessage("At least one product id is required.")
            .Must(list => list.All(id => !string.IsNullOrWhiteSpace(id)))
                .WithMessage("ProductIds cannot contain empty values.");
    }
}
