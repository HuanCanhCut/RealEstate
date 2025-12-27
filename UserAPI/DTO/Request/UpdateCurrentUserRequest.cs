using System.ComponentModel.DataAnnotations;

namespace UserAPI.DTO.Request
{
    public class UpdateCurrentUserRequest
    {
        public required string avatar_url { get; set; }
        public required string nickname { get; set; }
        public required string full_name { get; set; }

        [RegularExpression(@"^(0|\+84)(3|5|7|8|9)\d{8}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public required string phone_number { get; set; }
    }
}
