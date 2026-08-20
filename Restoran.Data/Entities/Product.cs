using System;
using System.Collections.Generic;
using System.Text;

namespace Restoran.Data.Entities
{
    public class Product
    {
      public  int Id { get; set; }
        public  string Name { get; set; } = string.Empty;
        
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public bool IsAvailable { get; set; }

    }
}
