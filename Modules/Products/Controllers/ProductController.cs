using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using APITEST.Common.Interfaces;
using APITEST.Modules.Products.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace APITEST.Modules.Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IValidator<ProductInsertDto> _productInsertValidator;
        private readonly IValidator<ProductUpdateDto> _productUpdateValidator;
        private readonly ICommonService<ProductDto, ProductInsertDto, ProductUpdateDto> _productService;

        public  ProductsController(
            IValidator<ProductInsertDto> productInsertValidator, 
            IValidator<ProductUpdateDto> productUpdateValidator,
            [FromKeyedServices("productService")] ICommonService<ProductDto, ProductInsertDto, ProductUpdateDto> productService
        )
        {
            _productInsertValidator = productInsertValidator;
            _productUpdateValidator = productUpdateValidator;
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<ProductDto>> GetProducts() => await _productService.FindAll();

        // GET: api/Products/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.FindById(id);

            return product == null ?
                        NotFound()
                        :
                        Ok(product);
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductDto>> CreateProductCategory(ProductInsertDto productoInsetDto)
        {
            var validationResult = await _productInsertValidator.ValidateAsync(productoInsetDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_productService.Validate(productoInsetDto))
            {
                return BadRequest(_productService.Errors);
            }

            var user = await _productService.Create(productoInsetDto);

            return CreatedAtAction(nameof(GetProduct), new { id = user.Id }, user);
        }

        // PUT | PATCH: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, ProductUpdateDto productUpdateDto)
        {
            var validationResult = await _productUpdateValidator.ValidateAsync(productUpdateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var product = await _productService.Update(id, productUpdateDto);

            if (!_productService.Validate(productUpdateDto))
            {
                return BadRequest(_productService.Errors);
            }

            return product == null ?
                        NotFound()
                        :
                        Ok(product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var delete = await _productService.Delete(id);

            return delete == null ? NotFound() : NoContent();
        }
    }
}
