using AdminAPI.DTO.Request;
using AdminAPI.Models;

namespace AdminAPI.Repositories.Interfaces
{
    public interface IContractResponsitory
    {
        int CountAll();
        List<ContractModel> GetContracts(int page, int perPage);

        ContractModel GetContractById(int id);

        bool UpdateStatus(int contractId, string status);

        bool DeleteContract(int id);

        List<ContractModel> FilterContracts(ContractFilterRequest filter);

        int CountFiltered(ContractFilterRequest filter);
    }
}
