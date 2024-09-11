using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using System.Linq.Expressions;

namespace NLayer.API.Filters
{
    //Diğer Filter senkron idi bunu asenktron yapacağız.
    //amacımız action'a girmeden id 'yi kontrol etmek.
    //public class NotFoundFilter<T> : IAsyncActionFilter where T : class
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
    {
        private readonly IService<T> _service;

        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idValue = context.ActionArguments.Values.FirstOrDefault(); //parametredeki id'yi aldık.
            if (idValue == null) 
            {
                await next.Invoke();
                return; //daha aşağıya gitme dön.
            }

            //id null değilse, id var ise 
            var id = (int)idValue;
            //var anyEntity = await _service.GetByIdAsync(id);
            var anyEntity = await _service.AnyAsync(x => x.Id == id);

            if (anyEntity) 
            {
                await next.Invoke();
                return;
            }

            context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name}({id}) not found"));
        }
    }
}
