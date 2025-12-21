using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.DTO.Response;
using AdminAPI.Repositories.Interfaces;

namespace AdminAPI.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly DbContext _dbContext;

        public AnalyticsRepository(DbContext context)
        {
            _dbContext = context;
        }

        public IAnalyticsRepository.Overview GetOverview(DateTime startDate, DateTime endDate)
        {
            try
            {
                string sql = @$"
                    SELECT
                        COUNT(id) AS total_posts,
                        COUNT(CASE WHEN post_status = 'approved' THEN 1 END) AS approved_posts,
                        COUNT(CASE WHEN post_status = 'pending' THEN 1 END) AS pending_posts,
                        (
                            SELECT 
                                COUNT(id) AS total_users
                            FROM users
                            WHERE created_at >= '{startDate:yyyy-MM-dd}'
                                AND created_at <  '{endDate:yyyy-MM-dd}'
                            ) AS total_users
                    FROM posts
                    WHERE created_at >= '{startDate:yyyy-MM-dd}'
                        AND created_at <  '{endDate:yyyy-MM-dd}';
                ";

                DataTable table = _dbContext.ExecuteQuery(sql);

                return new IAnalyticsRepository.Overview(
                    Convert.ToInt32(table.Rows[0]["total_posts"]),
                    Convert.ToInt32(table.Rows[0]["approved_posts"]),
                    Convert.ToInt32(table.Rows[0]["pending_posts"]),
                    Convert.ToInt32(table.Rows[0]["total_users"])
                );
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public List<AnalyticsCategoryPercent> GetCategoryPercentage()
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

                return table.ConvertTo<AnalyticsCategoryPercent>() ?? [];
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}