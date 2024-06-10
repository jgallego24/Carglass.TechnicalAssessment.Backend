using FluentValidation;

namespace Carglass.TechnicalAssessment.Backend.Dtos;

public class ProductDtoValidator : AbstractValidator<ProductDto>
{
    public ProductDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(default(int))
            .NotEmpty()
            .WithMessage("El Id del producto es necesario.");

        RuleFor(x => x.ProductName)
            .NotEmpty()
            .WithMessage("El tipo de producto es necesario.");
    }
}
