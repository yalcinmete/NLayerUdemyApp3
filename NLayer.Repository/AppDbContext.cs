using Microsoft.EntityFrameworkCore;
using NLayer.Core.Models;
using NLayer.Repository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {
        //startup dosyasından sql yolunu vermek için;
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }


        //ProductFeature kapatıp ProductFeauture sadece Product üzerinden yüklensin diyebilirsin:
        //var p = new Product() {ProductFeature = new ProductFeature(){}}
        public DbSet<ProductFeature> ProductFeatures { get; set; }


        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                                entityReference.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries()) 
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch(item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                            case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                                entityReference.UpdatedDate = DateTime.Now; 
                                break;
                            }
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Git IEntityTypeConfiguration implemente eden tüm classlardaki configurasyonları al.
            //GetExecutingAssembly() ; çalışmış olduğum assembly'yi tara.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            ////Tek tek vermek istersek , Ama yüzlerce configure dosyan olabileceği için yukarıdakini kullan. 
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());

           //Buradan da seeddata verebilirsin zaten buradan verebilirsin ama burayı kirletmemek için başka yerden(CategorySeed,ProductSeed) seeddataları verdik
            modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature()
            {
                Id = 1,
                Color = "Kırmızı",
                Height = 100,
                Width = 200,
                ProductId = 1,
            },
            new ProductFeature()
            {
                Id = 2,
                Color = "Mavi",
                Height = 300,
                Width = 500,
                ProductId = 2,
            });
        }
    }
}
