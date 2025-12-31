using AdminAPI.DTO.Response;
using AdminAPI.Models;
using AdminAPI.Repositories.Interfaces;
using AdminAPI.Services.Interfaces;
using static AdminAPI.Errors.Error;

namespace AdminAPI.Services
{
    public class ContractService : IContractSevice
    {
        private readonly IContractResponsitory _contractRepository;

        public ContractService(IContractResponsitory contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public ServiceResponsePagination<ContractModel> GetContracts(int page, int perPage)
        {
            try 
            {
                List<ContractModel> contracts = _contractRepository.GetContracts(page, perPage);

                int totalContracts = _contractRepository.CountAll();

                return new ServiceResponsePagination<ContractModel>
                {
                    data = contracts,
                    total = totalContracts,
                    count = contracts.Count
                };
            }
            catch (Exception ex)
            {
                if (ex is AppError) throw;

                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }
    }
}
