using System.Text.Json.Serialization;

namespace RealEstate.Models
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
        public string? role { get; set; } = "user";
        public string? address { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;
    }
}