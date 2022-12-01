using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace api.ViewModels
{
    public class ProductViewModel
    {

        public string _id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Category categoryId { get; set; }
        public decimal price { get; set; }
        public string currency { get; set; }
    }
}