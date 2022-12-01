using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
         private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<List<Category>> GetAllProducts()
        {
            // TODO Create View Model
            return await _categoryService.GetAllAsync();
        }
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Category>> Get(string id)
        {
            var category = await _categoryService.GetById(id);

            if (category is null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Category newCategory)
        {
            await _categoryService.CreateAsync(newCategory);

            return CreatedAtAction(nameof(GetAllProducts), newCategory);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Category updatedCategory)
        {
            //TODO add DTO
            var existingCategory = await _categoryService.GetById(id);

            if (existingCategory is null)
            {
                return NotFound("kategori bulunamadı");
            }

            updatedCategory._id = existingCategory._id;

            await _categoryService.UpdateAsync(id, updatedCategory);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingCategory = await _categoryService.GetById(id);

            if (existingCategory is null)
            {
                return NotFound("kategori bulunamadı");
            }

            await _categoryService.RemoveAsync(id);

            return NoContent();
        }
    }
}