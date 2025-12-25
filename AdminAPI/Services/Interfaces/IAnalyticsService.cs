using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.DTO.Response;
using AdminAPI.Models;

namespace AdminAPI.Services.Interfaces
{
    public interface IAnalyticsService
    {
        public AnalyticsOverviewResponse GetOverview(DateTime startDate, DateTime endDate);

        public List<PostLocationResponse> GetPostsLocation(DateTime startDate, DateTime endDate, int limit);

        public bool DeleteUser(int id);

    }
}