namespace AdminAPI.Models
{
        public class CategoryModel
        {
                public int id { get; set; }
                public required string name { get; set; }
                public required string key { get; set; }
                public DateTime created_at { get; set; } = DateTime.UtcNow;
                public DateTime updated_at { get; set; } = DateTime.UtcNow;

                // For analytics
                public required decimal percentage { get; set; }
        }
}