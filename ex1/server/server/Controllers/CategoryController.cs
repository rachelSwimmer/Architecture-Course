using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using server.DTOs;
using server.Interfaces;
using System.Text.Json;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        private readonly IDistributedCache _cache;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger, IDistributedCache cache)
        {
            _categoryService = categoryService;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll()
        {
            var cacheKey = "all-categories";

            // בדיקה אם יש נתונים ב-cache
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation("CACHE HIT");

                var categoriesFromCache =
                    JsonSerializer.Deserialize<IEnumerable<CategoryResponseDto>>(cachedData);

                return Ok(categoriesFromCache);
            }

            _logger.LogInformation("CACHE MISS");

            // קריאה ל-DB דרך השירות
            var categories = await _categoryService.GetAll();

            // שמירה ל-cache
            var serializedData = JsonSerializer.Serialize(categories);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300) // 5 דקות
            };

            await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);

            return Ok(categories);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound(new { message = $"Category with ID {id} not exist" });
            }
            return Ok(category);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddCategkory([FromBody] CategoryCreateDto createDto)
        {
            try
            {
                var category = await _categoryService.AddCategory(createDto);
                //return CreatedAtAction(nameof(GetById), new { id = category.Id, category });
                return Ok(category);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { messege = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var reasult = await _categoryService.DeleteCategory(id);
            if (!reasult)
            {
                return NotFound(new { message = $"Category with Id {id} not found" });
            }
            return NoContent();
        }
        [HttpPut("{categoryId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateCategory([FromRoute] int categoryId, [FromBody] CategoryUpdateDto updateDto)
        {
            try
            {
                var category = await _categoryService.UpdateCategory(categoryId, updateDto);
                return Ok(category);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { messege = ex.Message });
            }
        }


    }
}
