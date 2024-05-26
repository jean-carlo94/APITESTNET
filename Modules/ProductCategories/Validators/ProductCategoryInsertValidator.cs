using APITEST.Modules.ProductsCategory.DTOs;
using FluentValidation;

namespace APITEST.Modules.ProductCategories.Validators
{
    public class ProductCategoryInsertValidator: AbstractValidator<ProductCategoryInsertDto>
    {
        public ProductCategoryInsertValidator() {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, 50);

            RuleFor(x => x.Description)
                .NotEmpty();
        }
    }
}
