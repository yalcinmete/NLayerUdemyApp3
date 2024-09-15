using NLayer.Core.DTOs;

namespace NLayer.Web.Services
{
    public class CategoryApıService
    {
        private readonly HttpClient _httpClient;

        public CategoryApıService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<CategoryDto>>>("categories");
            return response.Data;
        }
    }
}
