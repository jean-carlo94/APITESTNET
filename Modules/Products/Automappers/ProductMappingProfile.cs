using AutoMapper;
using APITEST.Models;
using APITEST.Modules.Products.DTOs;

namespace APITEST.Modules.Products.Automappers
{
    public class StringToUpper: IValueConverter<string, string>
    {
        public string Convert(string sourceMember, ResolutionContext context)
        {
            return sourceMember.ToUpper();
        }
    }
    public class ProductMappingProfile: Profile
    {
        public ProductMappingProfile() 
        {
            CreateMap<ProductInsertDto, Product>()
                .ForMember(product => product.Name,
                            dto => dto.ConvertUsing(new StringToUpper(), dto => dto.Name)
                )
                .ForMember(product => product.Barcode,
                            dto => dto.ConvertUsing(new StringToUpper(), dto => dto.Barcode)
                );

            CreateMap<Product, ProductDto>();  

            CreateMap<ProductUpdateDto, Product>()
                .ForMember(product => product.Name,
                            dto => dto.ConvertUsing(new StringToUpper(), dto => dto.Name)
                )
                .ForMember(product => product.Barcode,
                            dto => dto.ConvertUsing(new StringToUpper(), dto => dto.Barcode)
                );
        }
    }
}
