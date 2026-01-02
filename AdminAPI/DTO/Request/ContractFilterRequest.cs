using System.ComponentModel.DataAnnotations;

namespace AdminAPI.DTO.Request
{
    public class ContractFilterRequest
    {
        public string? status { get; set; }          // approved, pending, rejected
        public DateTime? from_date { get; set; }     // yyyy-MM-dd
        public DateTime? to_date { get; set; }       // yyyy-MM-dd
        public string? keyword { get; set; }         // tên khách / môi giới / BĐS

        [Range(1, int.MaxValue, ErrorMessage = "Page phải lớn hơn 0.")]
        public int page { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "Per Page phải lớn hơn 0.")]
        public int per_page { get; set; } = 10;
    }
}
