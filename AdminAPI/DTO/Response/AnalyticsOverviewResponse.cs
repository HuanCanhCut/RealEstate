using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAPI.DTO.Response
{
    public class AnalyticsOverviewResponse
    {
        public required int total_posts { get; set; }
        public required double total_posts_growth_percent { get; set; }
        public required int approved_posts { get; set; }
        public required double approved_posts_growth_percent { get; set; }
        public required int pending_posts { get; set; }
        public required double pending_posts_growth_percent { get; set; }
        public required int users { get; set; }
        public required double users_growth_percent { get; set; }
    }
}