using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models.Enums;

namespace UserAPI.DTO.Request
{
    public enum Role
    {
        all,
        agent,
        user
    }

    public class GetPostRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page phải lớn hơn 0")]
        public int page { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "Per page phải lớn hơn 0")]
        public int per_page { get; set; } = 15;

        public ProjectType? project_type { get; set; }

        public decimal? min_price { get; set; }

        public decimal? max_price { get; set; }

        public string[]? property_categories { get; set; }

        public Role role { get; set; } = Role.all;
        public string? location { get; set; }
        public int? user_id { get; set; }

    }
}