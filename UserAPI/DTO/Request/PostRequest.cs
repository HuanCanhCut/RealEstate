using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models.Enums;

namespace UserAPI.DTO.Request
{
    public class PostDetail
    {
        [Required(ErrorMessage = "Bedrooms là bắt buộc")]
        public required int bedrooms { get; set; }

        [Required(ErrorMessage = "Bathrooms là bắt buộc")]
        public required int bathrooms { get; set; }

        public string? balcony { get; set; }
        public string? main_door { get; set; }

        [Required(ErrorMessage = "Legal documents là bắt buộc")]
        public required string legal_documents { get; set; }

        public string? interior_status { get; set; }

        [Required(ErrorMessage = "Area là bắt buộc")]
        public required int area { get; set; }

        [Required(ErrorMessage = "Price là bắt buộc")]
        public required string price { get; set; }

        public string? deposit { get; set; }
    }

    public class PostRequest
    {
        [Required(ErrorMessage = "Title là bắt buộc")]
        public required string title { get; set; }

        [Required(ErrorMessage = "Description là bắt buộc")]
        public required string description { get; set; }

        [Required(ErrorMessage = "Address là bắt buộc")]
        public required string address { get; set; }

        [Required(ErrorMessage = "Administrative address là bắt buộc")]
        public required string administrative_address { get; set; }

        [Required(ErrorMessage = "Project type là bắt buộc")]
        [EnumDataType(typeof(ProjectType))]
        public required ProjectType project_type { get; set; }

        [Required(ErrorMessage = "Category ID là bắt buộc")]
        public required int category_id { get; set; }

        [Required(ErrorMessage = "User ID là bắt buộc")]
        public required int user_id { get; set; }

        [Required(ErrorMessage = "Role là bắt buộc")]
        [EnumDataType(typeof(PostUserRole))]
        public required PostUserRole role { get; set; }

        public IFormFile[]? images { get; set; }

        public PostDetail details { get; set; }
    }
}