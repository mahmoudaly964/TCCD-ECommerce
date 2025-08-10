using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Products
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
