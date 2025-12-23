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
    }
}