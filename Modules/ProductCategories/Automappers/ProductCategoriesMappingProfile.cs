using APITEST.Models;
using APITEST.Modules.ProductsCategory.DTOs;
using AutoMapper;

namespace APITEST.Modules.ProductsCategory.Automappers
{
    public class NameToUpper: IValueConverter<string, string>
    {
        public string Convert(string sourceMember, ResolutionContext context)
        {
            return sourceMember.ToUpper();
        }
    }
    public class ProductCategoriesMappingProfile: Profile
    {
        public ProductCategoriesMappingProfile() 
        {
            CreateMap<ProductCategoryInsertDto, ProductCategory>()
                .ForMember(productCategory => productCategory.Name,
                            dto => dto.ConvertUsing(new NameToUpper(), dto => dto.Name)
                );

            CreateMap<ProductCategory, ProductCategoryDto>();  

            CreateMap<ProductCategoryUpdateDto, ProductCategory>()
                .ForMember(productCategory => productCategory.Name,
                            dto => dto.ConvertUsing(new NameToUpper(), dto => dto.Name)
                );
        }
    }
}
