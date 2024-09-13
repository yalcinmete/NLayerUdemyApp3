using NLayer.Core.DTOs;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Services
{
    public interface IProductService : IService<Product>
    {
        //Task<List<ProductWithCategoryDto>> GetProductsWithCategory();
        //CustomResponseDto dönelim ki tam API 'nin istediği nesneyi dönelim.

        //MVC projesine geçtik. CustomResponseDto dönmemize gerek yok.
        //Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory();

        Task<List<ProductWithCategoryDto>> GetProductsWithCategory();
    }
}
