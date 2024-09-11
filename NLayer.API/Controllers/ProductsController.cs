using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{

    //public class ProductController : ControllerBase
    public class ProductsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        //private readonly IService<Product> _service;
        private readonly IProductService _service;

        public ProductsController(IMapper mapper, IService<Product> service, IProductService productService)
        {
            _mapper = mapper;
            _service = productService;
            //_productService = productService;
        }

        //GET api/products/GetProductsWithCategory
        //[HttpGet("GetProductsWithCategory")]
        //Diğer Get api/product/ ile çakışmaması için HttpGet("GetProductsWithCategory") ekliyoruz
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            //return CreateActionResult(await _productService.GetProductsWithCategory());
            return CreateActionResult(await _service.GetProductsWithCategory());
        }

        //GET api/products
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();
            var productsDtos = _mapper.Map<List<ProductDto>>(products.ToList());
            //return Ok(CustomResponseDto<List<ProductDto>>.Succes(200, productsDtos));
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Succes(200, productsDtos));
        }

        //[NotFoundFilter],  [ValidateFilter] gibi Filter sınıfından miras almıyor. IAsyncActionFilter sınıfından miras aldığı için ve DI'inda parametre geçtiği için bunu direkt [NotFoundFilter] şeklinde kullanamıyoruz.[ServiceFilter] yardımıyla kullanabiliyoruz.
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        //GET /api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            var productsDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Succes(200, productsDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            var productsDto = _mapper.Map<ProductDto>(product);
            //return Ok(CustomResponseDto<List<ProductDto>>.Succes(200, productsDtos));
            return CreateActionResult(CustomResponseDto<ProductDto>.Succes(201, productsDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productDto));

            return CreateActionResult(CustomResponseDto<NoContentDto>.Succes(204));
        }


        //DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);

            //product olup olmaması kontrolu yapılmalı ama burayı kirletmemek adına bu kontrolu başka yerden yapacağız.
            //if (product == null)
            //{
            //    return CreateActionResult(CustomResponseDto<List<NoContentDto>>.Fail(404,"bu id'ye sahip ürün bulunamadı"));
            //}

            await _service.RemoveAsync(product);
            return CreateActionResult(CustomResponseDto<List<NoContentDto>>.Succes(204));
        }
    }
}
