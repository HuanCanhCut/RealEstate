using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAPI.DTO.Response
{
    public class PostLocationResponse
    {
        public required string address { get; set; }
        public int post_count { get; set; }
        public double growth_rate { get; set; }
        public double percentage { get; set; }
    }
}