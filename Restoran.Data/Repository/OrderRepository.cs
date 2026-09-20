using Microsoft.EntityFrameworkCore;
using Restoran.Data.Entities;
using Restoran.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restoran.Data.Repository
{
    public class OrderRepository:GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(RestoranDbContext context):base(context)
        {
            
        }
        public async Task<Order?> GetOpenOrderByTableIdAsync(int id)
        { 
            var result = await _context.Orders.FirstOrDefaultAsync(o => o.TableId == id && o.PaymentStatus == PaymentStatus.Unpaid );
            return result;
        }
        public async Task<Order?> GetByIdWithDetailsAsync(int id)
        {
            var result = await _context.Orders.Include(o => o.OrderItems).
                ThenInclude(oi => oi.Product).
                FirstOrDefaultAsync(oid => oid.Id == id);
            return result;
        }

    }
}
