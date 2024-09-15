using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerService.Services
{
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productService) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _productRepository = productService;
        }

        //public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        //{
        //    var products = await _productRepository.GetProductsWithCategory();
        //    var productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
        //    //CustomResponseDto<List<ProductWithCategoryDto>> ile Api'nin istediği nesneyi dönmüş oldum.
        //    return CustomResponseDto<List<ProductWithCategoryDto>>.Succes(200, productsDto);
        //}

        //Daha önceden API ile çalıştığımız için bu servisten CustomResponseDto<List<ProductWithCategoryDto>> dönüyorduk.Artık MVC projemizde customresponseDto'ya ihtiyacımız yok.Direk  List<ProductWithCategoryDto> ile çalışabiliriz.
        //MVC-API Haberleşmesinde Task<List<ProductWithCategoryDto>> => Task<CustomResponseDto<List<ProductWithCategoryDto>>> çevirdik.Tabi interface'inde de güncelleştirmeyi yapman gerekir.
        public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
            var products = await _productRepository.GetProductsWithCategory();
            var productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            //CustomResponseDto<List<ProductWithCategoryDto>> ile Api'nin istediği nesneyi dönmüş oldum.
            return CustomResponseDto<List<ProductWithCategoryDto>>.Succes(200,productsDto);
        }
    }
}
