using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserAPI.DTO.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public required string email { get; set; }
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public required string password { get; set; }
    }
}