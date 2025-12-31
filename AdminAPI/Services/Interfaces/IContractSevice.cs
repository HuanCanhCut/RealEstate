using AdminAPI.DTO.Response;
using AdminAPI.Models;

namespace AdminAPI.Services.Interfaces
{
    public interface IContractSevice
    {
        ContractModel GetContractById(int id);
        public ServiceResponsePagination<ContractModel> GetContracts(int page, int perPage);
    }
}
