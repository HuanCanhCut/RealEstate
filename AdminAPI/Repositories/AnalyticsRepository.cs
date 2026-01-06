using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.DTO.Response;
using AdminAPI.Models;
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

        public List<IAnalyticsRepository.PostLocation> GetPostsLocation(DateTime startDate, DateTime endDate, int limit, List<string>? locations = null)
        {
            try
            {
                string whereClause = "";

                if (locations != null && locations.Any())
                {
                    whereClause = $"AND posts.administrative_address IN ({string.Join(",", locations.Select(x => $"'{x}'"))})";
                }

                string sql = $@"
                    SELECT
                        posts.administrative_address,
                        COUNT(id) AS total_post
                    FROM posts
                    WHERE created_at >= '{startDate:yyyy-MM-dd}'
                        AND created_at <  '{endDate:yyyy-MM-dd}'
                    {whereClause}
                    GROUP BY posts.administrative_address
                    ORDER BY total_post DESC
                    LIMIT {limit};
                ";

                DataTable table = _dbContext.ExecuteQuery(sql);

                List<IAnalyticsRepository.PostLocation> postLocations = [];

                foreach (DataRow row in table.Rows)
                {
                    // Skip if address is null or empty
                    string? address = Convert.ToString(row["administrative_address"]);
                    if (address == null || address == "") { continue; }

                    postLocations.Add(new IAnalyticsRepository.PostLocation(
                        address,
                        Convert.ToInt32(row["total_post"])
                    ));
                }

                return postLocations;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public PostOverviewResponse GetPostsOverview()
        {
            try
            {
                string sql = @$"
                    SELECT
                        COUNT(id) AS total_posts,
                        COUNT(CASE WHEN post_status = 'approved' THEN 1 END) AS approved_count,
                        COUNT(CASE WHEN post_status = 'pending' THEN 1 END) AS pending_count,
                        COUNT(CASE WHEN post_status = 'rejected' THEN 1 END) AS rejected_count
                    FROM posts;
                ";

                DataTable table = _dbContext.ExecuteQuery(sql);

                return new PostOverviewResponse(
                    Convert.ToInt32(table.Rows[0]["total_posts"]),
                    Convert.ToInt32(table.Rows[0]["approved_count"]),
                    Convert.ToInt32(table.Rows[0]["pending_count"]),
                    Convert.ToInt32(table.Rows[0]["rejected_count"])
                );
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}