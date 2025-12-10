using System.Text.Json.Serialization;
using UserAPI.Models.Enums;

namespace UserAPI.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public required string full_name { get; set; }
        public required string email { get; set; }

        [JsonIgnore]
        public string? password { get; set; }
        public required string nickname { get; set; }
        public string? phone_number { get; set; }
        public string? avatar { get; set; }
        public UserRole? role { get; set; } = UserRole.user;
        public string? address { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;


        //các trường dành cho việc join
        public long post_count { get; set; }
    }
}