using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.DTO.Response;
using AdminAPI.Repositories.Interfaces;
using AdminAPI.Services.Interfaces;
using static AdminAPI.Repositories.AnalyticsRepository;
using static AdminAPI.Errors.Error;

namespace AdminAPI.Services
{
    public class AnalyticsService(IAnalyticsRepository analyticsRepository) : IAnalyticsService
    {
        private readonly IAnalyticsRepository _analyticsRepository = analyticsRepository;

        public AnalyticsOverviewResponse GetOverview(DateTime startDate, DateTime endDate)
        {
            try
            {
                int dateDiff = (endDate - startDate).Days + 1;

                IAnalyticsRepository.Overview current = _analyticsRepository.GetOverview(startDate, endDate);
                IAnalyticsRepository.Overview lookBack = _analyticsRepository.GetOverview(startDate.AddDays(-dateDiff), endDate.AddDays(-dateDiff));

                return new AnalyticsOverviewResponse
                {
                    total_posts = current.total_posts,
                    total_posts_growth_percent = lookBack.total_posts == 0 ? current.total_posts * 100 : (double)(current.total_posts - lookBack.total_posts) / lookBack.total_posts * 100,
                    approved_posts = current.approved_posts,
                    approved_posts_growth_percent = lookBack.approved_posts == 0 ? current.approved_posts * 100 : (double)(current.approved_posts - lookBack.approved_posts) / lookBack.approved_posts * 100,
                    pending_posts = current.pending_posts,
                    pending_posts_growth_percent = lookBack.pending_posts == 0 ? current.pending_posts * 100 : (double)(current.pending_posts - lookBack.pending_posts) / lookBack.pending_posts * 100,
                    users = current.users,
                    users_growth_percent = lookBack.users == 0 ? current.users * 100 : (double)(current.users - lookBack.users) / lookBack.users * 100
                };
            }
            catch (Exception ex)
            {
                if (ex is AppError)
                {
                    throw;
                }

                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }
    }
}