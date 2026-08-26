using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restoran.Data.Repository
{
    public class GenericRepository<T>:IGenericRepository<T> where T : class
    {
        protected readonly RestoranDbContext _context;
        public GenericRepository(RestoranDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
        return   await _context.Set<T>().ToListAsync(); 
        }
        public async Task<T?> GetByIdAsync(int id) 
        {
            return await _context.Set<T>().FindAsync(id);

            
         
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;

            
           

        }
        public void Update(T entity)
        { 
            _context.Set<T>().Update(entity);
        }
        public void Delete(int id) 
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }
        public async Task<int> SaveChangesAsync() 
        { 
            return await _context. SaveChangesAsync();
        }


    }
}
