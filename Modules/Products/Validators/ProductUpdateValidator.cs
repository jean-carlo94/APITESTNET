using FluentValidation;
using APITEST.Modules.Products.DTOs;

namespace APITEST.Modules.Products.Validators
{
    public class ProductUpdateValidator: AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateValidator() {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, 100);

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.Barcode)
                .NotEmpty();

            RuleFor(x => x.Price)
                .NotEmpty();

            RuleFor(x => x.CurrentStock)
                .NotEmpty();

            RuleFor(x => x.ProductCategoryId)
                .NotEmpty();
        }
    }
}
