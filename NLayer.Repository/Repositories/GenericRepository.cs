using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    //Ef core classlarla çalışıyor.T'nin ne olduğunu (where T :class) belirtmezsen anlamaz. 
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        //readonly ; ya başlangıçta değer ataması yapabilirsin ya da contructor'da değer ataması yapabilirsin
        //Entity'lere özgü repository(ProductRepository) oluşturacağımız ve _context nesnesini kullanacağımız için protected yaptık.
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression)
        {
            //AsNoTracking() diyorum ki EF çekmiş olduğu dataları belleğe alıp izlemesin daha performanslı calıssın. Entityleri sadece çekiyoruz üzerinden işlem yapmıyoruz bu nedenle izlemesine gerek yok.
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            //FindAsync(birden fazla id girişi yapabilirsin.Tabloda Birden fazla id olabilir)
            return await _dbSet.FindAsync(id);
        }

        public void Remove(T entity)
        {
            //_dbSet.Remove(entity) ile _context.Entry(entity).State = EntityState.Deleted demek aynı şey. Sadece izlenen entity'inin  state 'ini deleted flag'ı ile işaretlediğimiz için async metoduna ihtiyaç yok(ef'da da yok). Ardından SaveChange() denildiğinde veri database'den silinir.
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);   
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }
    }
}
