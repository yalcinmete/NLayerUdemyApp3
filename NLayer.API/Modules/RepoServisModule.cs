using Autofac;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Repository;
using NLayerService.Mapping;
using NLayerService.Services;
using System.Reflection;
using Module = Autofac.Module;
    
namespace NLayer.API.Modules
{
    public class RepoServisModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Program.cs'de sonu Repository ve service ile bitmeyenler için tek tek girmemiz gerekiyor.Burada önce somut olanı(GenericRepository) sonra soyut olanı eklediğimize dikkat et. 
            //bunlar generic olduğu için RegisterGeneric kullandık.
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

            //program.cs'de Unitofwork generic olmadığı için RegisterType kullandık.
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();





            //Api katmanını al.Şuan çalışmış olduğumuz katman olduğu için GetExecutingAssembly().
            var apiAssembly = Assembly.GetExecutingAssembly();

            //Repository katmanını al.GetAssembly içine repository katmanından herhangibir class verebilirsin.
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));

            //Service katmanını al.GetAssembly içine repository katmanından herhangibir class verebilirsin.
            var serviceAssembly = Assembly.GetAssembly(typeof(MapProfile));

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

            //AddScoped : Bir request başladı bitene kadar aynı instance(nesneyi) kullansın.
            //Transient : Herhangi bir classın constructure'ında o interface nerede geçildiyse her seferinde yeni bir instance(nesne) oluştur.
            //InstancePerLifetimeScope => Scope
            //InstancePerDependency   => transient 'e karşılık gelir

        }
    }
}
