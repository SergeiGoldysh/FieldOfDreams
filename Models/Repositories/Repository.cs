using Microsoft.EntityFrameworkCore;
using Models.Common;
using Models.Repositories.Interfaces;
using Models.SqlServer;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Models.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T item)
        {
            DbSet<T> dbSet = _context.Set<T>();
            if(dbSet == default(DbSet<T>)) 
            {
                return default(T);
            }
            T result = (await dbSet.AddAsync(item)).Entity;
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<List<T>> AddAllAsync(IEnumerable<T> items)
        {
            List<T> result = new List<T>();

            DbSet<T> dbSet = _context.Set<T>();

            if (dbSet == default(DbSet<T>))
            {
                return default(List<T>);
            }
            foreach (T item in items)
            {
                T entity = (await dbSet.AddAsync(item)).Entity;
                result.Add(entity);
            }
            await _context.SaveChangesAsync();
            return result;

        }



        public async Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, object>> include)
        {
            DbSet<T> dbSet = _context.Set<T>();

            if (dbSet == default(DbSet<T>))
            {
                return default(List<T>);
            }

            return await dbSet.Include(include).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            DbSet<T> dbSet = _context.Set<T>();
            if (dbSet == default(DbSet<T>))
            {
                return default(T);
            }
            T item = await dbSet.FirstOrDefaultAsync(x=>x.Id == id);
            return item;
        }

        public async Task UpdateAsync(T item)
        {
            DbSet<T> dbSet = _context.Set<T>();

            if (dbSet == default(DbSet<T>))
            {
                return;
            }

            dbSet.Update(item);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T item)
        {
            DbSet<T> dbSet = _context.Set<T>();

            if (dbSet == default(DbSet<T>))
            {
                return;
            }

            dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            DbSet<T> dbSet = _context.Set<T>();

            if (dbSet == default(DbSet<T>))
            {
                return default(List<T>);
            }
            return await dbSet.ToListAsync();
        }
    }
}
