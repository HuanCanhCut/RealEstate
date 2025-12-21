using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.DTO.Response;

namespace AdminAPI.Services.Interfaces
{
    public interface IAnalyticsService
    {
        public AnalyticsOverviewResponse GetOverview(DateTime startDate, DateTime endDate);

        public List<AnalyticsCategoryPercent> GetCategoryPercentage();
    }
}