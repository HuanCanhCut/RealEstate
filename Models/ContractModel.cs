
namespace RealEstate.Models
{
    public class ContractModel
    {
        public int id { get; set; }
        public int customer_id { get; set; }
        public int agent_id { get; set; }
        public required string customer_cccd { get; set; }
        public required string customer_phone { get; set; }
        public int post_id { get; set; }
        public required string amount { get; set; }
        public required string commission { get; set; }
        public string status { get; set; } = "pending"; // 'pending', 'approved', 'rejected'
        public string duration { get; set; } = "2 năm"; // '2 năm', '5 năm', '10 năm'
        public required string clause { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        // Navigation properties for joins
        public UserModel? Customer { get; set; }
        public UserModel? Agent { get; set; }
        public PostModel? Post { get; set; }
    }
}