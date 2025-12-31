using AdminAPI.Models;

namespace AdminAPI.Repositories.Interfaces
{
    public interface IContractResponsitory
    {
        public int CountAll();
        public List<ContractModel> GetContracts(int page, int perPage);

        ContractModel GetContractById(int id);

        bool UpdateStatus(int contractId, string status);


    }
}
