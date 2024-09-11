using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayerService.Mapping;
using NLayerService.Services;
using NLayerService.Validations;
using System.Reflection;

namespace NLayer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddControllers();
            //builder.Services.AddControllers().AddFluentValidation(x=>x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
            builder.Services.AddControllers(options =>  options.Filters.Add(new ValidateFilterAttribute()) ).AddFluentValidation(x=>x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());

            //FluentValidation Library-2 Kendi filterimizin nesnesinin dönmesini istiyorsak default dönen deðeri pasif'e çekmeliyiz.Bu durum sadece API için geçerli.
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Filterinda DI geçiyorsan program.cs içerisinde bunu belirtmelisin.
            builder.Services.AddScoped(typeof(NotFoundFilter<>));

            //Video80Migrations
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //IGenericRepository birden çok T almýþ olsaydý : IGenericRepository<,,,> þeklinde yazýcaktýk.
            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
            builder.Services.AddAutoMapper(typeof(MapProfile));

            builder.Services.AddScoped<IProductRepository,ProductRepository>();
            builder.Services.AddScoped<IProductService,ProductService>();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            //Video80 Migrations.
            builder.Services.AddDbContext<AppDbContext>(x =>
            {
                x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
                {
                    //option sebebi ; AppDbContext assembly'si API katmanýnda olmadýðý için, AppDbContext'in yerini EF'a söylüyoruz.
                    option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCustomException(); //GlobalExceptionHandler

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
