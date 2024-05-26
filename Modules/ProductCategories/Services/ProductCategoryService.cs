using Microsoft.EntityFrameworkCore;
using AutoMapper;
using APITEST.Common.Interfaces;
using APITEST.Models;
using APITEST.Modules.ProductsCategory.DTOs;

namespace APITEST.Modules.ProductsCategory.Services
{
    public class ProductCategoryService : ICommonService<ProductCategoryDto, ProductCategoryInsertDto, ProductCategoryUpdateDto>
    {
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IMapper _mapper;
        public List<string> Errors { get; }

        public ProductCategoryService(
            IRepository<ProductCategory> ProductCategoryRepository,
            IMapper mapper
        )
        {
            _productCategoryRepository = ProductCategoryRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }

        public async Task<IEnumerable<ProductCategoryDto>> FindAll()
        {
            var productsCategory = await _productCategoryRepository.GetAll();

            return productsCategory.Select(productCategory => _mapper.Map<ProductCategoryDto>(productCategory));
        }

        public async Task<ProductCategoryDto> FindById(int Id)
        {
            var productCategory = await _productCategoryRepository.GetById(Id);

            if (productCategory != null)
            {
                var productCategoryDto = _mapper.Map<ProductCategoryDto>(productCategory);
                return productCategoryDto;
            }

            return null;
        }

        public async Task<ProductCategoryDto> Create(ProductCategoryInsertDto productCategoryInsert)
        {
            var productCategory = _mapper.Map<ProductCategory>(productCategoryInsert);

            await _productCategoryRepository.Create(productCategory);
            await _productCategoryRepository.Save();

            var productCategoryDto = _mapper.Map<ProductCategoryDto>(productCategory);
            return productCategoryDto;
        }

        public async Task<ProductCategoryDto> Update(int Id, ProductCategoryUpdateDto productCategoryUpdate)
        {
            var productCategory = await _productCategoryRepository.GetById(Id);

            if (productCategory == null)
            {
                return null;
            }

            productCategory = _mapper.Map<ProductCategoryUpdateDto, ProductCategory>(productCategoryUpdate, productCategory);

            _productCategoryRepository.Update(productCategory);

            try
            {
                await _productCategoryRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }

            var productCategoryDto = _mapper.Map<ProductCategoryDto>(productCategory);
            return productCategoryDto;
        }

        public async Task<ProductCategoryDto> Delete(int id)
        {
            var productCategory = await _productCategoryRepository.GetById(id);

            if (productCategory == null)
            {
                return null;
            }

            _productCategoryRepository.Delete(productCategory);
            await _productCategoryRepository.Save();

            var productCategoryDto = _mapper.Map<ProductCategoryDto>(productCategory);
            return productCategoryDto;
        }

        public bool Validate(ProductCategoryInsertDto productCategoryInsert)
        {
            if (
                _productCategoryRepository.Search(
                    productCategory => productCategory.Name == productCategoryInsert.Name.ToUpper()
                )
                .Count() > 0
            )
            {
                Errors.Add("La Cageoria de producto ya existe");
                return false;
            }
            return true;
        }

        public bool Validate(ProductCategoryUpdateDto productCategoryUpdate)
        {
            if (_productCategoryRepository.Search(
                productCategory => productCategory.Name == productCategoryUpdate.Name.ToUpper()
                && productCategoryUpdate.Id != productCategory.Id
                )
                .Count() > 0
            )
            {
                Errors.Add("La Cageoria de producto ya existe");
                return false;
            }
            return true;
        }
    }
}
