using Restoran.Data.Entities;
using Restoran.Data.Enums;
using Restoran.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restoran.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGenericRepository<Product> _genericProductRepository;
         
        public OrderService(IOrderRepository orderRepository, IGenericRepository<Product> genericProductRepository)
        {
            _orderRepository = orderRepository;
            _genericProductRepository = genericProductRepository;
        }
        public async Task<Order> CreateOrderAsync(int tableId, int waiterId)
        {
         var result = await _orderRepository.GetOpenOrderByTableIdAsync(tableId);
            if (result != null)
            {
                throw new ArgumentException("Hata! Bu masa dolu");
            }
            Order order = new Order
            {
                TableId = tableId,
                WaiterId = waiterId,
                PaymentStatus = PaymentStatus.Unpaid


            };
            await _orderRepository.AddAsync(order);

            await _orderRepository.SaveChangesAsync();
            return order;

        }
        public async Task<Order> AddOrderItemAsync(int orderId, int productId, int quantity, string? note)
        {
          var order =  await _orderRepository.GetByIdAsync(orderId);
            if (order == null) 
            {
                throw new ArgumentException("Siparis Bulunamadi");
            }
            var product = await _genericProductRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new ArgumentException("Urun  Bulunamadi");
            }
            OrderItem orderItem = new OrderItem
            {
                OrderId = orderId,
                ProductId = productId,
                Quantity = quantity,
                Note = note,
                UnitPrice = product.Price

            };
            order.OrderItems .Add(orderItem);
            await _orderRepository.SaveChangesAsync();
            return order;

        }
    }
}
