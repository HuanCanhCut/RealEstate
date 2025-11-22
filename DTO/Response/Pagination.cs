using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.DTO.Response
{
    public class Pagination
    {
        public required int total { get; set; }
        public required int count { get; set; }
        public required int current_page { get; set; }
        public required int total_pages { get; set; }
        public required int per_page { get; set; }
    }
}