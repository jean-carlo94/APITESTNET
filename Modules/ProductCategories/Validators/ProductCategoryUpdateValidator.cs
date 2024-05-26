using APITEST.Modules.ProductsCategory.DTOs;
using FluentValidation;

namespace APITEST.Modules.ProductCategories.Validators
{
    public class ProductCategoryUpdateValidator: AbstractValidator<ProductCategoryUpdateDto>
    {
        public ProductCategoryUpdateValidator() {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, 50);

            RuleFor(x => x.Description)
                .NotEmpty();
        }
    }
}
