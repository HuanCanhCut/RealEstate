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

        public ContractModel GetContractById(int id)
        {
            try
            {
                ContractModel contract = _contractRepository.GetContractById(id);

                if (contract == null)
                {
                    throw new NotFoundError("Không tìm thấy hợp đồng");
                }

                return contract;
            }
            catch (Exception ex)
            {
                if (ex is AppError) throw;

                throw new InternalServerError(ex.Message);
            }
        }

        public bool UpdateStatus(int contractId, string status)
        {
            try
            {
                // 1️⃣ Check contract tồn tại
                ContractModel contract = _contractRepository.GetContractById(contractId);

                if (contract == null)
                {
                    throw new NotFoundError("Không tìm thấy hợp đồng");
                }

                // 2️⃣ Validate status
                List<string> allowStatus = new()
        {
            "approved",
            "rejected"
        };

                if (!allowStatus.Contains(status))
                {
                    throw new BadRequestError("Trạng thái hợp đồng không hợp lệ");
                }

                // 3️⃣ Update
                bool result = _contractRepository.UpdateStatus(contractId, status);

                if (!result)
                {
                    throw new InternalServerError("Cập nhật trạng thái thất bại");
                }

                return true;
            }
            catch (Exception ex)
            {
                if (ex is AppError) throw;
                throw new InternalServerError(ex.Message);
            }
        }

    }
}
