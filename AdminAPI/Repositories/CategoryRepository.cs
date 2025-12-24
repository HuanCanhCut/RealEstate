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

        public int CreateCategory(string name, string key, int userId)
        {
            try
            {
                // key is a reserved word in MySQL, so we need to use backticks to escape it
                string sql = @$"
                        INSERT INTO categories (`name`, `key`, user_id)
                        VALUES ('{name}', '{key}', {userId});

                        SELECT LAST_INSERT_ID();
                ";

                Console.WriteLine(sql);

                int lastInsertId = Convert.ToInt32(_dbContext.ExecuteScalar(sql));

                return lastInsertId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CategoryModel? GetCategoryById(int id)
        {
            try
            {
                string sql = @$"
                    SELECT * FROM categories WHERE id = {id}
                ";

                DataTable table = _dbContext.ExecuteQuery(sql);

                return table.ConvertTo<CategoryModel>()?.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CategoryModel? GetCategoryByNameAndKey(string name, string key)
        {
            try
            {
                string sql = @$"
                    SELECT * FROM categories WHERE `name` = '{name}' AND `key` = '{key}'
                ";

                DataTable table = _dbContext.ExecuteQuery(sql);

                return table.ConvertTo<CategoryModel>()?.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateCategory(int id, string name, string key, int userId)
        {
            try
            {
                string sql = @$"
                    UPDATE 
                        categories 
                    SET 
                        `name` = '{name}', 
                        `key` = '{key}', 
                        `user_id` = {userId} 
                    WHERE id = {id};
                ";

                int affectedRows = _dbContext.ExecuteNonQuery(sql);

                return affectedRows;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int DeleteCategory(int id)
        {
            try
            {
                string sql = @$"
                    DELETE FROM categories WHERE id = {id}
                ";

                int affectedRows = _dbContext.ExecuteNonQuery(sql);

                return affectedRows;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}