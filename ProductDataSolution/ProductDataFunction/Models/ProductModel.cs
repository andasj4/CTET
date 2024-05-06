using System;
using System.Collections.Generic;

namespace ProductDataFunction.Models
{
    public class ProductModel
    {
        public List<Product> Products { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string ProductNo { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
