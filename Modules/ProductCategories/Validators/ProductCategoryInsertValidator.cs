using FluentValidation;
using APITEST.Modules.ProductsCategory.DTOs;

namespace APITEST.Modules.ProductCategories.Validators
{
    public class ProductCategoryInsertValidator : AbstractValidator<ProductCategoryInsertDto>
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
