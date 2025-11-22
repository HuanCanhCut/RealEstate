
namespace RealEstate.Models
{
    public class PostModel
    {
        public int id { get; set; }
        public required string title { get; set; }
        public required string description { get; set; }
        public required string address { get; set; }
        public required string administrative_address { get; set; }
        public string? project_type { get; set; } = "sell";
        public string? images { get; set; }
        public string? post_status { get; set; } = "pending";
        public string? status { get; set; } = "Chưa bàn giao"; // 'Chưa bàn giao', 'Đã bàn giao'
        public int category_id { get; set; }
        public int user_id { get; set; }
        public string? role { get; set; } = "user"; // 'user' or 'agent'
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        // Navigation properties for joins
        public CategoryModel? Category { get; set; }
        public UserModel? User { get; set; }
        public PostDetailModel? PostDetail { get; set; }
    }
}