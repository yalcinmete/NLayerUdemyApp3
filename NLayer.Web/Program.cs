using Autofac.Extensions.DependencyInjection;
using Autofac;
using NLayer.Web.Modules;
using Microsoft.EntityFrameworkCore;
using NLayer.Repository;
using System.Reflection;
using NLayerService.Mapping;
using FluentValidation.AspNetCore;
using NLayerService.Validations;
using NLayer.Web.Services;

namespace NLayer.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
            builder.Services.AddAutoMapper(typeof(MapProfile));

            builder.Services.AddDbContext<AppDbContext>(x =>
            {
                x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
                {
                    //option sebebi ; AppDbContext assembly'si API katmanýnda olmadýðý için, AppDbContext'in yerini EF'a söylüyoruz.
                    option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
                });
            });

            //MVC-API haberleþtirilmesi.Program.cs e yazarak(tabi öncesinde ProductApiService DI geçtik.) ProductApiService herhangibir classýn ctor'unda geçip istediðim gibi kullanabilirim. 
            builder.Services.AddHttpClient<ProductApiService>(opt =>
            {
                opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
            });

            //MVC-API haberleþtirilmesi.Program.cs e yazarak(tabi öncesinde CategoryApýService DI geçtik.) CategoryApýService herhangibir classýn ctor'unda geçip istediðim gibi kullanabilirim. 
            builder.Services.AddHttpClient<CategoryApýService>(opt =>
            {
                opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
            });

            //Filter'a DI geçiyorsan program.cs'de bunu belirtmelisin.
            builder.Services.AddScoped(typeof(NotFoundFilter<>));

            //AutoFac
            builder.Host.UseServiceProviderFactory
                (new AutofacServiceProviderFactory());

            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServisModule()));
            //birden fazla module olursa aþaðýya doðru kopyala yapýstýr yaz.

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}