using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.DTO.Response;
using AdminAPI.Repositories.Interfaces;
using AdminAPI.Services.Interfaces;
using static AdminAPI.Repositories.AnalyticsRepository;
using static AdminAPI.Errors.Error;
using AdminAPI.Models;

namespace AdminAPI.Services
{
    public class AnalyticsService(IAnalyticsRepository analyticsRepository, IPostService postService) : IAnalyticsService
    {
        private readonly IAnalyticsRepository _analyticsRepository = analyticsRepository;
        private readonly IPostService _postService = postService;

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

        public List<PostLocationResponse> GetPostsLocation(DateTime startDate, DateTime endDate, int limit)
        {
            try
            {
                int dateDiff = (endDate - startDate).Days + 1;

                List<IAnalyticsRepository.PostLocation> currentPostLocations = _analyticsRepository.GetPostsLocation(startDate, endDate, limit);

                List<string> locations = currentPostLocations.Select(postLocation => postLocation.address).Distinct().ToList() ?? [];

                // Get the look back post the same locations with the current post locations
                List<IAnalyticsRepository.PostLocation> lookBackPostLocations = _analyticsRepository.GetPostsLocation(startDate.AddDays(-dateDiff), endDate.AddDays(-dateDiff), limit, locations);

                int totalPostsCount = _postService.CountAll();


                List<PostLocationResponse>? response = currentPostLocations?.Select((currentPostLocation, index) =>
                {
                    // Find the look back post count for the current post location
                    int lookBackPostCount = lookBackPostLocations.Find(lockBackPostLocation => lockBackPostLocation.address == currentPostLocation.address)?.post_count ?? 0;

                    double growth_rate = lookBackPostCount == 0 ? currentPostLocation.post_count * 100 : (double)(currentPostLocation.post_count - lookBackPostCount) / lookBackPostCount * 100;

                    double percentage = (double)currentPostLocation.post_count / totalPostsCount * 100;

                    return new PostLocationResponse
                    {
                        address = currentPostLocation.address,
                        post_count = currentPostLocation.post_count,
                        growth_rate = growth_rate,
                        percentage = percentage
                    };
                })?.ToList();

                return response ?? [];
            }
            catch (Exception ex)
            {
                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }
    }
}