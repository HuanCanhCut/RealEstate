using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAPI.DTO.Response
{
    public class PostOverviewResponse
    {
        public int total { get; set; }
        public int approved_count { get; set; }
        public int pending_count { get; set; }
        public int rejected_count { get; set; }

        public PostOverviewResponse(int total, int approved_count, int pending_count, int rejected_count)
        {
            this.total = total;
            this.approved_count = approved_count;
            this.pending_count = pending_count;
            this.rejected_count = rejected_count;
        }

    }
}