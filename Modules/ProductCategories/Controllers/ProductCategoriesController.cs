using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APITEST.Common.Interfaces;
using FluentValidation;
using APITEST.Modules.ProductsCategory.DTOs;

namespace APITEST.Modules.ProductsCategory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : Controller
    {
        private readonly IValidator<ProductCategoryInsertDto> _productCategoryInsertValidator;
        private readonly IValidator<ProductCategoryUpdateDto> _productCategoryUpdateValidator;
        private readonly ICommonService<ProductCategoryDto, ProductCategoryInsertDto, ProductCategoryUpdateDto> _productCategoryService;

        public ProductCategoriesController(
            IValidator<ProductCategoryInsertDto> productCategoryInsertValidator,
            IValidator<ProductCategoryUpdateDto> productCategoryUpdateValidator,
            [FromKeyedServices("productCategoryService")] ICommonService<ProductCategoryDto, ProductCategoryInsertDto, ProductCategoryUpdateDto> productCategoryService
         )
        {
            _productCategoryInsertValidator = productCategoryInsertValidator;
            _productCategoryUpdateValidator = productCategoryUpdateValidator;
            _productCategoryService = productCategoryService;
        }

        // GET: api/ProductCategories
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories() => await _productCategoryService.FindAll();

        // GET: api/ProductCategories/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductCategoryDto>> GetProductCategory(int id)
        {
            var productCategory = await _productCategoryService.FindById(id);

            return productCategory == null ?
                        NotFound()
                        :
                        Ok(productCategory);
        }

        // POST: api/ProductCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductCategoryDto>> CreateProductCategory(ProductCategoryInsertDto productoCategoryInsetDto)
        {
            var validationResult = await _productCategoryInsertValidator.ValidateAsync(productoCategoryInsetDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_productCategoryService.Validate(productoCategoryInsetDto))
            {
                return BadRequest(_productCategoryService.Errors);
            }

            var user = await _productCategoryService.Create(productoCategoryInsetDto);

            return CreatedAtAction(nameof(GetProductCategories), new { id = user.Id }, user);
        }

        // PUT | PATCH: api/ProductCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductCategoryDto>> UpdateProductCategory(int id, ProductCategoryUpdateDto productCategoryUpdateDto)
        {
           var validationResult = await _productCategoryUpdateValidator.ValidateAsync(productCategoryUpdateDto);

           if (!validationResult.IsValid)
           {
               return BadRequest(validationResult.Errors);
           }

            var productCategory = await _productCategoryService.Update(id, productCategoryUpdateDto);

            if (!_productCategoryService.Validate(productCategoryUpdateDto))
            {
                return BadRequest(_productCategoryService.Errors);
            }

            return productCategory == null ?
                        NotFound()
                        :
                        Ok(productCategory);
        }

        // DELETE: api/ProductCategories/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteProductCategory(int id)
        {
            var delete = await _productCategoryService.Delete(id);

            return delete == null ? NotFound() : NoContent();
        }
    }
}
