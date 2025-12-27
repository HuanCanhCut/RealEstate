using System.ComponentModel.DataAnnotations;

namespace AdminAPI.DTO.Request
{
    public class PaginationRequest
    {

        [Range(1, int.MaxValue, ErrorMessage = "Page phải lớn hơn 0.")]
        public required int page { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Per Page phải lớn hơn 0.")]
        public required int per_page { get; set; }
    }
}
