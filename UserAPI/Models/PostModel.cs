using UserAPI.Models.Enums;

namespace UserAPI.Models
{
    public class PostModel
    {
        public int id { get; set; }
        public required string title { get; set; }
        public required string description { get; set; }
        public required string address { get; set; }
        public required string administrative_address { get; set; }
        public ProjectType? project_type { get; set; } = ProjectType.sell;
        public string? images { get; set; }
        public PostEnum? post_status { get; set; } = PostEnum.pending;
        public string? status { get; set; } = "Chưa bàn giao"; // 'Chưa bàn giao', 'Đã bàn giao'
        public int category_id { get; set; }
        public int user_id { get; set; }
        public PostUserRole? role { get; set; } = PostUserRole.user;
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        // Navigation properties for joins
        public CategoryModel? category { get; set; }
        public UserModel? user { get; set; }
        public List<PostDetailModel>? json_post_detail { get; set; }
    }
}