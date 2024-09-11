using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerService.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            //Product'ı ProductDto'ya dönüştürmek,ProductDto'yu da Product'a dönüştürmek istiyorum
            CreateMap<Product,ProductDto>().ReverseMap();
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<ProductFeature,ProductFeatureDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product,ProductWithCategoryDto>();
            CreateMap<Category,CategoryWithProductsDto>();
        }
    }
}
