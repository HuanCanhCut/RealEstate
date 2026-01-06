using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.Models.Enums;

namespace AdminAPI.DTO.Request
{
    public class GetPostsRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int page { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int per_page { get; set; }
        [EnumDataType(typeof(PostEnum))]
        public PostEnum? post_status { get; set; }
        [EnumDataType(typeof(ProjectType))]
        public ProjectType? project_type { get; set; }
        public int? category_id { get; set; }
        public string? search { get; set; }
    }
}