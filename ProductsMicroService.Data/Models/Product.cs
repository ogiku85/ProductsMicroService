using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsMicroService.Data.Models
{
    public class Product : Audit
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string URL { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
    }
}
