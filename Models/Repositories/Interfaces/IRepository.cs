using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repositories.Interfaces
{
    public interface IRepository <T> where T : BaseEntity
    {
        public Task<T> AddAsync(T item);
        public Task<List<T>> AddAllAsync(IEnumerable<T> items);
        Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> include = null);
        public Task<List<T>> GetAllAsync();
        public Task<T> GetByIdAsync(int id);
        public Task UpdateAsync(T item); 
        public Task DeleteAsync(T item); 

    }
}
