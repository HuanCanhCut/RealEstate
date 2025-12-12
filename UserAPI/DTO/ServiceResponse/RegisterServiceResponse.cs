using UserAPI.DTO.Response;
using UserAPI.Models;

namespace UserAPI.DTO.ServiceResponse
{
        public class RegisterServiceResponse
        {
                public required UserModel user { get; set; }
                public required string access_token { get; set; }
                public required string refresh_token { get; set; }
        }
}