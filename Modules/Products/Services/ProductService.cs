using Microsoft.EntityFrameworkCore;
using AutoMapper;
using APITEST.Common.Interfaces;
using APITEST.Models;
using APITEST.Modules.Products.DTOs;

namespace APITEST.Modules.Products.Services
{
    public class ProductService : ICommonService<ProductDto, ProductInsertDto, ProductUpdateDto>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        public List<string> Errors { get; }

        public ProductService(
            IRepository<Product> ProductRepository,
            IMapper mapper
        )
        {
            _productRepository = ProductRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }

        public async Task<IEnumerable<ProductDto>> FindAll()
        {
            var products = await _productRepository.GetAll();

            return products.Select(product => _mapper.Map<ProductDto>(product));
        }

        public async Task<ProductDto> FindById(int Id)
        {
            var product = await _productRepository.GetById(Id);

            if (product != null)
            {
                var prpductDto = _mapper.Map<ProductDto>(product);
                return prpductDto;
            }

            return null;
        }

        public async Task<ProductDto> Create(ProductInsertDto productInsert)
        {
            var product = _mapper.Map<Product>(productInsert);

            await _productRepository.Create(product);
            await _productRepository.Save();

            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        public async Task<ProductDto> Update(int Id, ProductUpdateDto productUpdate)
        {
            var product = await _productRepository.GetById(Id);

            if (product == null)
            {
                return null;
            }

            product = _mapper.Map<ProductUpdateDto, Product>(productUpdate, product);

            _productRepository.Update(product);

            try
            {
                await _productRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }
        public async Task<ProductDto> Delete(int Id)
        {
            var product = await _productRepository.GetById(Id);

            if (product == null)
            {
                return null;
            }

            _productRepository.Delete(product);
            await _productRepository.Save();

            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        public bool Validate(ProductInsertDto productInsert)
        {
            if (
                _productRepository.Search(
                    product => product.Name == productInsert.Name.ToUpper()
                )
                .Count() > 0
            )
            {
                Errors.Add("Este producto ya existe");
                return false;
            }

            if (
                _productRepository.Search(
                    product => product.Barcode == productInsert.Barcode.ToUpper()
                )
                .Count() > 0
            )
            {
                Errors.Add("El codigo del producto ya existe");
                return false;
            }

            return true;
        }

        public bool Validate(ProductUpdateDto productUpdate)
        {
            if (_productRepository.Search(
                product => product.Name == productUpdate.Name.ToUpper()
                && product.Id != product.Id
                )
                .Count() > 0
            )
            {
                Errors.Add("Este producto ya existe");
                return false;
            }

            if (_productRepository.Search(
                product => product.Barcode == productUpdate.Barcode.ToUpper()
                && product.Id != product.Id
                )
                .Count() > 0
            )
            {
                Errors.Add("El codigo del producto ya existe");
                return false;
            }
            return true;
        }
    }
}
