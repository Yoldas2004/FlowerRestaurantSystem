    using System;
using System.Collections.Generic;
using System.Text;

namespace Restoran.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        void Update (T entity);
        void Delete (int id);
        Task<int> SaveChangesAsync();
    }
}
