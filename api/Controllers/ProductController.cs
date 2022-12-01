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
            //tüm ürünleri 5 dakikalığına cache'e ekleme
            var cacheData = _cacheService.GetData<List<Product>>("AllProducts");
            if (cacheData != null)
            {
                return cacheData;
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            cacheData = await _productService.GetAllAsync();
            _cacheService.SetData<IEnumerable<Product>>("AllProducts", cacheData, expirationTime);


            // TODO Create View Model
            // IMPORTANT sadece test için oluşturulmuştur. !!!
            return cacheData;
        }
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<ProductViewModel>> Get(string id)
        {
            Product product;
            var cacheData = _cacheService.GetData<List<Product>>("AllProducts");



            if (cacheData != null)
            {
                //cache var
                product = cacheData.Where(x => x._id == id).FirstOrDefault(); //filter
                if (product == null)
                {
                    //check db
                    product = await _productService.GetById(id);
                    if (product is null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        //cache'de yok ama veritabanında var
                        cacheData.Add(product);
                        var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
                        _cacheService.SetData<IEnumerable<Product>>("AllProducts", cacheData, expirationTime);

                    }
                }
            }
            else
            {
                //cache boş
                product = await _productService.GetById(id);
                if (product is null)
                {
                    return NotFound();
                }
                //cache boş tüm ürünleri cache'e ekle
                //TODO geçici çözümdür. düzletilecek
                var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
                cacheData = await _productService.GetAllAsync();
                _cacheService.SetData<IEnumerable<Product>>("AllProducts", cacheData, expirationTime);


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

        [HttpGet("/list")]
        public async Task<ActionResult<List<ProductViewModel>>> GetByName(string name)
        {
            List<Product> products;
            var cacheData = _cacheService.GetData<List<Product>>("AllProducts");
            if (cacheData != null)
            {
                //cache var
                products = cacheData.Where(x => x.name.Contains(name)).ToList(); //filter
                if (products == null)
                {
                    //check db
                    products = await _productService.GetByName(name);
                    if (products is null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        //cache'de yok ama veritabanında var
                        cacheData.AddRange(products);
                        var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
                        _cacheService.SetData<IEnumerable<Product>>("AllProducts", cacheData, expirationTime);

                    }
                }
            }
            else
            {
                //cache boş
                products = await _productService.GetByName(name);
                if (products is null)
                {
                    return NotFound();
                }
                //cache boş tüm ürünleri cache'e ekle
                //TODO geçici çözümdür. düzletilecek
                var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
                cacheData = await _productService.GetAllAsync();
                _cacheService.SetData<IEnumerable<Product>>("AllProducts", cacheData, expirationTime);


            }

            List<ProductViewModel> searchResult = new List<ProductViewModel>();
            foreach (var product in products)
            {
                var category = await _categoryService.GetById(product.categoryId);

                ProductViewModel productViewModel = new ProductViewModel();
                productViewModel._id = product._id;
                productViewModel.categoryId = category;
                productViewModel.currency = product.currency;
                productViewModel.description = product.description;
                productViewModel.name = product.name;
                productViewModel.price = product.price;
                searchResult.Add(productViewModel);
            }


            //TODO use mapper

            return searchResult;
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