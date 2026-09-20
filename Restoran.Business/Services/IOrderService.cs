using Restoran.Data.Entities;
using Restoran.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restoran.Business.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int tableId , int waiterId);
        Task<Order> AddOrderItemAsync(int orderId,int productId,int quantity,string? note);
        Task<Order> PayOrderAsync(int orderId, PaymentMethod method);
    
    }
}
