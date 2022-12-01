using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class ProductDTO
    {

        public string name { get; set; }
        public string description { get; set; }
        public string categoryId { get; set; }
        public decimal price { get; set; }
        public string currency { get; set; }
    }
}