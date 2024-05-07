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
        public string ProductNo { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }        
        public DateTime ModifiedDate { get; set; } = DateTime.Now;       
    }
}
