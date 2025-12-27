using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.Models;

namespace AdminAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        public List<CategoryModel> GetCategories(int? currentUserId);
        public CategoryModel CreateCategory(string name, string key);
        public CategoryModel UpdateCategory(int id, string name, string key);
        public void DeleteCategory(int id);
    }
}