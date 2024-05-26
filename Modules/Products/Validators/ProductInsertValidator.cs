using FluentValidation;
using APITEST.Modules.Products.DTOs;

namespace APITEST.Modules.Products.Validators
{
    public class ProductInsertValidator: AbstractValidator<ProductInsertDto>
    {
        public ProductInsertValidator() {
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
