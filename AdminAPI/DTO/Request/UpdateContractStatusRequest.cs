using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminAPI.DTO.Request
{
    public class UpdateContractStatusRequest
    {
        public string status { get; set; } = string.Empty;
    }
}

