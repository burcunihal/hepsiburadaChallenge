using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models
{
    public class Product
    {
        /*
        {
"_id":ObjectId("b2dc7b494d9e2c75d64cc722ade4e63”),
"name":"Döner",
"description": "1 Porsiyon yaprak döner",
"categoryId":"10aeda2dfe374764e33eb14b208b262f",
"price": 25.90,
"currency: "TL"
}
        */
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string categoryId { get; set; }
        public decimal price { get; set; }
        public string currency { get; set; }

        public Product()
        {
            _id = ObjectId.GenerateNewId().ToString();
        }
    }
}