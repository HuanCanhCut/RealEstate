using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.DTO.Response;
using AdminAPI.Models;

namespace AdminAPI.Repositories.Interfaces
{
    public interface IAnalyticsRepository
    {
        // RECORD
        public record Overview(int total_posts, int approved_posts, int pending_posts, int users);
        public record PostLocation(string address, int post_count);

        // METHODS
        public Overview GetOverview(DateTime startDate, DateTime endDate);
        public List<PostLocation> GetPostsLocation(DateTime startDate, DateTime endDate, int limit, List<string>? locations = null);

    }
}