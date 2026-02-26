using server.DTOs;
using server.Interfaces;
using server.Models;
using server.Repositories;

namespace server.Services
{
    public class CategoryService : ICategoryService

    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }
        public async Task<CategoryResponseDto> AddCategory(CategoryCreateDto categoryDto)
        {
            _logger.LogInformation("Post /add category called");
            
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
            };
            try
            {
                var createdCategory = await _categoryRepository.AddCategory(category);
                _logger.LogInformation("category created with ID: {categoryId}", createdCategory.Id);
                return MapToResponeseDto(createdCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding category");
                throw;
            }
        }

        public async Task<bool> DeleteCategory(int id)
        {
            _logger.LogInformation("Post / delete category {categoryId} deleted", id);
            try
            {
                return await _categoryRepository.DeleteCategory(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category");
                throw;
            }
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAll()
        {
            _logger.LogInformation("Get / all category called");
            try
            {
                var categories = await _categoryRepository.GetAll();
                return categories.Select(MapToResponeseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving categories");
                throw;
            }
            
        }

        public async Task<CategoryResponseDto> GetById(int id)
        {
            _logger.LogInformation("Get / get category : {categoryId} called",id);

            try
            {
                var category = await _categoryRepository.GetById(id);
                return category != null ? MapToResponeseDto(category) : null;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving category by ID");
                throw;
            }
           
        }

        public async Task<CategoryResponseDto> UpdateCategory(int categoryId, CategoryUpdateDto categoryDto)
        {
            _logger.LogInformation("PUT / update category : {categoryId} called", categoryId);
            try
            {
                var existingCategory = await _categoryRepository.GetById(categoryId);
                if (existingCategory == null)
                    return null;
                existingCategory.Name = categoryDto.Name;
                existingCategory.Description = categoryDto.Description;
                var updatedCategory = await _categoryRepository.UpdateCategory(existingCategory);
                return MapToResponeseDto(updatedCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category");
                throw;
            }
            
        }

        private static CategoryResponseDto MapToResponeseDto(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            };

        }

        
    }
}
