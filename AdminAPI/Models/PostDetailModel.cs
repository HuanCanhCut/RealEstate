namespace AdminAPI.Models
{
    public class PostDetailModel
    {
        public int id { get; set; }
        public int post_id { get; set; }
        public int? bedrooms { get; set; }
        public int? bathrooms { get; set; }
        public string? balcony { get; set; }
        public string? main_door { get; set; }
        public string? legal_documents { get; set; }
        public string? interior_status { get; set; }
        public int? area { get; set; }
        public string? price { get; set; }
        public string? deposit { get; set; }
        public string? type { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;
    }
}