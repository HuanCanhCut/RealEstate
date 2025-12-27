using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.Models;

namespace AdminAPI.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public List<CategoryModel> GetCategories();
        public int CreateCategory(string name, string key);
        public CategoryModel? GetCategoryById(int id);
        public CategoryModel? GetCategoryByNameAndKey(string name, string key);
        public int UpdateCategory(int id, string name, string key);
        public int DeleteCategory(int id);
    }
}