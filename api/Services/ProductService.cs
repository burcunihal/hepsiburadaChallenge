using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Services
{
    public class ProductService
    {
        //TODO Create Inteface
        private readonly IMongoCollection<Product> _products;

        public ProductService(IOptions<DbSettings> appDbSettings)
        {
            var mongoClient = new MongoClient(appDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(appDbSettings.Value.DatabaseName);
            _products = mongoDatabase.GetCollection<Product>("products");
        }
        public async Task<List<Product>> GetAllAsync() => await _products.Find(_ => true).ToListAsync();
        public async Task<Product?> GetById(string id) => await _products.Find(p => p._id.Equals(id)).FirstOrDefaultAsync();
        public async Task CreateAsync(Product newProduct) => await _products.InsertOneAsync(newProduct);
        public async Task UpdateAsync(string id, Product updatedProduct) => await _products.ReplaceOneAsync( p => p._id == id, updatedProduct);
        public async Task RemoveAsync(string id) => await _products.DeleteOneAsync(p=> p._id == id);

    }
}