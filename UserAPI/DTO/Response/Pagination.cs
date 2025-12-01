using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAPI.DTO.Response
{
    public class Pagination(int total, int count, int current_page, int per_page)
    {
        public int total { get; set; } = total;
        public int count { get; set; } = count;
        public int current_page { get; set; } = current_page;
        public int total_pages { get; set; } = (int)Math.Ceiling(total / (double)per_page);
        public int per_page { get; set; } = per_page;
    }
}