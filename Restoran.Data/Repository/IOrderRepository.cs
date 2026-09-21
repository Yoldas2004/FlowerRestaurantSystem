using Restoran.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restoran.Data.Repository
{
    public interface IOrderRepository:IGenericRepository<Order>
    {
        Task<Order?> GetOpenOrderByTableIdAsync(int id);
        Task<Order?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Order>> GetAllUnPaidsAsync();
    }
}
