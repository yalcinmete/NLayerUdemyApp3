using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs;
using NLayerService.Exceptions;
using System.Text.Json;

namespace NLayer.API.Middlewares
{
    //bir extension metot yazabilmek için classın da static olmalıdır
    public static class UseCustomExceptionHandler
    {
        //Kullanacağımız UseCustomException middleware'ne hata fırlatmayı service katmanında yapıyoruz !.
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(
                //Kendi modelimi dönmek istediğim için config ile içerisine giriyorum.
                // Bu bir API uygulaması olduğu için JSON döneceğim bu nedenle context'in tipini belirleyelim
                config =>
                {
                    // Bu bir API uygulaması olduğu için JSON döneceğim bu nedenle context'in tipini belirleyelim
                    //Run() => middleware'i sonlandırıcı metotdur. 
                    config.Run(async context =>
                    {
                        context.Response.ContentType = "application/json";

                        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>(); //uygulamadan fırlatılan hatayı alıyoruz.

                        //Clientın hatasından ben bir hata fırlatabilirim veya sunucu kaynaklı da bir hata fırlatılabilir
                        var statusCode = exceptionFeature.Error switch
                        {
                            ClientSideException => 400,
                            NotFoundException => 404,
                            _ => 500
                        };
                        context.Response.StatusCode = statusCode;

                        var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);
                        //Response bir tip. Ben bunu JSON'a serilaze etmem lazım. CustomMiddleware olusturdugumuz icin json dönüştürmesi yapmak zorundayız diğer api controller'da bu otomatik yapılıyor.
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));

                    });

                });
        }
    }
}
