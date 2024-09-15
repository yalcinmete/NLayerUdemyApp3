using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Web.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        //Artık API üzerinden servislerimizi çağıracağımız için bunlara gerek yok.API içerisinde mapper configurasyonlarını da yaptık.
        //private readonly IProductService _service;
        //private readonly ICategoryService _categoryService;
        //private readonly IMapper _mapper;

        //public ProductsController(IProductService service, ICategoryService categoryService, IMapper mapper)
        //{
        //    _service = service;
        //    _categoryService = categoryService;
        //    _mapper = mapper;
        //}

        private readonly ProductApiService _productApiService;
        private readonly CategoryApıService _categoryApiService;

        public ProductsController(ProductApiService productApiService, CategoryApıService categoryApiService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
        }

        public async Task<IActionResult> Index()
        {
            //CustomResponseDto bize lazım değil bize CustomResponseDto'daki data lazım.
            //Eğer projende sadece web uygulaması ise zaten CustomResponseDto olusturmana gerek yok.API için CustomResponseDto olusturmustuk.Projemizdeki Servis katmanını değiştirmek istemediğimiz için CustomResponseDto 'dan devam ettik.

            //Sonradan CustomResponse kullanmama kararı aldık . Direkt GetProductsWithCategory()'den productdto ile devam ettik.
            //return View(await _service.GetProductsWithCategory());

            //MVC-API Haberleşmesinde API CustomResponse kullandığı için;.
            //return View((await _service.GetProductsWithCategory()).Data);

            //Artık API'den çekiyoruz.
            return View(await _productApiService.GetProductsWithCategoryAsync());
        }

        public async Task<IActionResult> Save()
        {
            //var categories = await _categoryService.GetAllAsync();

            //var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            //Artık API'den çekiyoruz.
            var categoriesDto = await _categoryApiService.GetAllAsync();

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {

            if (ModelState.IsValid) 
            {
                //await _service.AddAsync(_mapper.Map<Product>(productDto));

                //Artık API'den çekiyoruz.
                await _productApiService.SaveAsync(productDto);


                return RedirectToAction(nameof(Index));
            }
            //var categories = await _categoryService.GetAllAsync();

            //categories ile IEnumrable dönüyor ben list istiyorum.O halde categories.ToList()
            //var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            //Artık API'den çekiyoruz.
            var categoriesDto = await _categoryApiService.GetAllAsync();

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");
            return View();
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            //var product = await _service.GetByIdAsync(id);

            //var categories = await _categoryService.GetAllAsync();

            //var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            var product = await _productApiService.GetByIdAsync(id);

            var categoriesDto = await _categoryApiService.GetAllAsync();

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", product.CategoryId);

            //return View(_mapper.Map<ProductDto>(product));
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                //await _service.UpdateAsync(_mapper.Map<Product>(productDto));
                
                //Artık API'den çekiyoruz.
                await _productApiService.UpdateAsync(productDto);

                return RedirectToAction(nameof(Index));
            }
            //var categories = await _categoryService.GetAllAsync();

            //var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            var categoriesDto = await _categoryApiService.GetAllAsync();

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", productDto.CategoryId);

            return View(productDto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            //var product = await _service.GetByIdAsync(id);

            //Artık API'den çekiyoruz ama burada Servis önceden product nesnesi istiyordu şuan bizim api'deki metota id versen yeterli.
            //var product = await _productApiService.GetByIdAsync(id);

            //await _service.RemoveAsync(product);

            await _productApiService.RemoveAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }

}
