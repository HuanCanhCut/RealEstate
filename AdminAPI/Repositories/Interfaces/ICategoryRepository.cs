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
    }
}