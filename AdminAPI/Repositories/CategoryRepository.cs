using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.Models;
using AdminAPI.Repositories.Interfaces;

namespace AdminAPI.Repositories
{
    public class CategoryRepository(DbContext context) : ICategoryRepository
    {
        private readonly DbContext _dbContext = context;

        public List<CategoryModel> GetCategories()
        {
            try
            {
                string sql = @$"
                    SELECT
                        categories.*,
                        COUNT(posts.id) * 100.0 / (SELECT COUNT(*) FROM posts) AS percentage
                    FROM
                        categories
                    JOIN posts ON posts.category_id = categories.id
                    GROUP BY categories.id
                ";

                DataTable table = _dbContext.ExecuteQuery(sql);

                return table.ConvertTo<CategoryModel>() ?? [];
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}