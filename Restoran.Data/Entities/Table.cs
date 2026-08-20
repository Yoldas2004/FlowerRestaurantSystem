using Restoran.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restoran.Data.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public TableStatusType TableStatus { get; set; }
    }
}
