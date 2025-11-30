
namespace UserAPI.Models
{
    public class FavoriteModel
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int post_id { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        // Navigation properties for joins
        public UserModel? json_user { get; set; }
        public PostModel? json_post { get; set; }
    }
}