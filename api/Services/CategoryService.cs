using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryService(IOptions<DbSettings> appDbSettings)
        {
            var mongoClient = new MongoClient(appDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(appDbSettings.Value.DatabaseName);
            _categories = mongoDatabase.GetCollection<Category>("categories");
        }
        public async Task<List<Category>> GetAllAsync() => await _categories.Find(_ => true).ToListAsync();
        public async Task<Category?> GetById(string id) => await _categories.Find(c=> c._id.Equals(id)).FirstOrDefaultAsync();
        public async Task CreateAsync(Category newCategory) => await _categories.InsertOneAsync(newCategory);
        public async Task UpdateAsync(string id, Category updatedCategory) => await _categories.ReplaceOneAsync( c => c._id == id, updatedCategory);
        public async Task RemoveAsync(string id) => await _categories.DeleteOneAsync(c=> c._id == id);
    }
}