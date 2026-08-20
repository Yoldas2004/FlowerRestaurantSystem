using Restoran.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restoran.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; } = null!;
        public int WaiterId { get; set; }
        public User Waiter { get; set; } = null!;   
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; } 
         

    }
}
