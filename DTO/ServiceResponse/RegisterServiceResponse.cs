using RealEstate.DTO.Response;
using RealEstate.Models;

namespace RealEstate.DTO.ServiceResponse
{
    public class RegisterServiceResponse
    {
        public required UserModel user { get; set; }
        public required MetaToken meta { get; set; }
    }
}