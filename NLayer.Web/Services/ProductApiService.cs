using NLayer.Core.DTOs;
using NLayer.Core.Models;

namespace NLayer.Web.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductWithCategoryDto>> GetProductsWithCategoryAsync()
        {
            ////httpClient.GetFromJsonAsync kullanımı gelmeden önce ;
            //var response2 = await _httpClient.GetAsync("products/GetProductsWithCategory");
            //response2.Content.ReadAsStringAsync....
            ////Okuduktan sonra string'i de JSON'a cast ediyorduk.


            //products / GetProductsWithCategory'e istek yapıyorum.JSON olarak okuyorum.CustomResponseDto<List<ProductWithCategoryDto döneceğini belirtiyorum.
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<ProductWithCategoryDto>>>("products/GetProductsWithCategory");

            return response.Data;
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<ProductDto>>($"products/{id}");

            //hata olma durumunda return response.Error dönebilirsin.
            //if (response.Errors.Any())
            //{
            //logla,hatayı dön vs...
            //}
            //else

            return response.Data;
        }

        public async Task<ProductDto> SaveAsync (ProductDto newProduct)
        {
            //Eskiden post yapıp gelen datayı json'a çeviriyorduk.

            //JSON olarak post yap.
            var response = await _httpClient.PostAsJsonAsync("products", newProduct);

            if (!response.IsSuccessStatusCode) return null; //eğer başarısız olursa null dön.

            //başarılı olursa CustomResponseDto<ProductDto> tipi gelicek sana bunu CustomResponseDto<ProductDto> tipte oku.
            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<ProductDto>>();

            return responseBody.Data;
        }

        public async Task<bool> UpdateAsync(ProductDto newProduct)
        {
            //JSON olarak put yap.
            var response = await _httpClient.PutAsJsonAsync("products", newProduct);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            //JSON olarak put yap.
            var response = await _httpClient.DeleteAsync($"products/{id}");

            return response.IsSuccessStatusCode;
        }

    }
}
