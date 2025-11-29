using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserAPI.DTO.Request
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public required string email { get; set; }

        [Required(ErrorMessage = "Mật khẩu cũ là bắt buộc")]
        public required string password { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự")]
        public required string new_password { get; set; }
    }
}