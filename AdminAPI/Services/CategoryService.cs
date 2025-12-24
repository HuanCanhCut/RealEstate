using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.Models;
using AdminAPI.Models.Enums;
using AdminAPI.Repositories.Interfaces;
using AdminAPI.Services.Interfaces;
using static AdminAPI.Errors.Error;

namespace AdminAPI.Services
{
    public class CategoryService(ICategoryRepository categoryRepository, IUserRepository userRepository) : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public List<CategoryModel> GetCategories(int? currentUserId)
        {
            try
            {
                bool isAdmin = true;

                if (currentUserId == null)
                {
                    isAdmin = false;
                }
                else
                {
                    UserModel? user = _userRepository.GetUserById(currentUserId.Value);

                    if (user?.role != UserRole.admin)
                    {
                        isAdmin = false;
                    }
                }

                List<CategoryModel> categories = _categoryRepository.GetCategories();

                // set percentage to 0 for non-admin users
                if (!isAdmin)
                {
                    categories = categories.Select(c => new CategoryModel
                    {
                        id = c.id,
                        key = c.key,
                        name = c.name,
                        percentage = 0,
                        created_at = c.created_at,
                        updated_at = c.updated_at
                    }).ToList();
                }

                return categories;
            }
            catch (Exception ex)
            {
                if (ex is AppError)
                {
                    throw;
                }

                throw new InternalServerError(ex.Message + ex.StackTrace);

            }
        }

        public CategoryModel CreateCategory(string name, string key, int userId)
        {
            try
            {
                CategoryModel? existingCategory = _categoryRepository.GetCategoryByNameAndKey(name, key);

                if (existingCategory != null)
                {
                    throw new ConflictError("Danh mục đã tồn tại");
                }

                int insertedId = _categoryRepository.CreateCategory(name, key, userId);

                if (insertedId == 0)
                {
                    throw new BadRequestError("Không thể tạo danh mục");
                }

                CategoryModel? category = _categoryRepository.GetCategoryById(insertedId);

                return category!;
            }
            catch (Exception ex)
            {
                if (ex is AppError)
                {
                    throw;
                }

                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }

        public CategoryModel? UpdateCategory(int id, string name, string key, int userId)
        {
            try
            {
                CategoryModel? existingCategory = _categoryRepository.GetCategoryById(id);

                if (existingCategory == null)
                {
                    throw new NotFoundError("Danh mục không tồn tại");
                }

                int rowsAffected = _categoryRepository.UpdateCategory(id, name, key, userId);

                if (rowsAffected == 0)
                {
                    throw new BadRequestError("Không thể cập nhật danh mục");
                }

                CategoryModel? category = _categoryRepository.GetCategoryById(id);

                return category;
            }
            catch (Exception ex)
            {
                if (ex is AppError)
                {
                    throw;
                }

                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }
    }
}