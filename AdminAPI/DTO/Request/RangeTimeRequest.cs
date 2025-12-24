using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAPI.DTO.Request
{
    public class RangeTimeRequest
    {
        public required DateTime start_date { get; set; }
        public required DateTime end_date { get; set; }
    }
}