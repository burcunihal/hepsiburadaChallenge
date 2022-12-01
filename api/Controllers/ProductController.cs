using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Interfaces;
using api.Models;
using api.Services;
using api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly ICacheService _cacheService;


        public ProductController(ProductService productService, CategoryService categoryService, ICacheService cacheService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<List<Product>> GetAllProducts()
        {

            
            // TODO Create View Model
            // IMPORTANT sadece test için oluşturulmuştur. !!!
            return await _productService.GetAllAsync();
        }
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<ProductViewModel>> Get(string id)
        {
            Product product;
            var cacheData = _cacheService.GetData<Product>("product");
            if (cacheData != null)
            {
                product = cacheData;//.Where(x => x._id == id).FirstOrDefault();
            }
            else
            {
                product = await _productService.GetById(id);
                if (product is null)
                {
                    return NotFound();
                }
                var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
                _cacheService.SetData<Product>("product", product, expirationTime);

            }


            var category = await _categoryService.GetById(product.categoryId);

            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel._id = product._id;
            productViewModel.categoryId = category;
            productViewModel.currency = product.currency;
            productViewModel.description = product.description;
            productViewModel.name = product.name;
            productViewModel.price = product.price;
            //TODO use mapper

            return productViewModel;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductDTO newProduct)
        {
            Product product = new Product();
            product.categoryId = newProduct.categoryId;
            product.currency = newProduct.currency;
            product.description = newProduct.description;
            product.name = newProduct.name;
            product.price = newProduct.price;
            await _productService.CreateAsync(product);

            return CreatedAtAction(nameof(GetAllProducts), newProduct);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Product updatedProduct)
        {
            //TODO add DTO
            var existingProduct = await _productService.GetById(id);

            if (existingProduct is null)
            {
                return NotFound("ürün bulunamadı");
            }

            updatedProduct._id = existingProduct._id;

            await _productService.UpdateAsync(id, updatedProduct);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingProduct = await _productService.GetById(id);

            if (existingProduct is null)
            {
                return NotFound("ürün bulunamadı");
            }

            await _productService.RemoveAsync(id);

            return NoContent();
        }
    }
}