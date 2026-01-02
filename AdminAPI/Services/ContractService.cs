using AdminAPI.DTO.Request;
using AdminAPI.DTO.Response;
using AdminAPI.Models;
using AdminAPI.Repositories.Interfaces;
using AdminAPI.Services.Interfaces;
using static AdminAPI.Errors.Error;
using System.Text;

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

        public bool DeleteContract(int id)
        {
            try
            {
                bool result = _contractRepository.DeleteContract(id);

                if (!result)
                    throw new NotFoundError("Hợp đồng không tồn tại hoặc đã bị xoá");

                return true;
            }
            catch (Exception ex)
            {
                if (ex is AppError) throw;
                throw new InternalServerError(ex.Message);
            }
        }

        public ServiceResponsePagination<ContractModel> FilterContracts(ContractFilterRequest filter)
        {
            try
            {
                List<ContractModel> contracts = _contractRepository.FilterContracts(filter);
                int totalContracts = _contractRepository.CountFiltered(filter);

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

                throw new InternalServerError(ex.Message);
            }
        }

        public byte[] ExportContractsCsv(ContractFilterRequest filter)
        {
            // Lấy toàn bộ danh sách theo filter
            filter.page = 1;
            filter.per_page = int.MaxValue;

            var contracts = _contractRepository.FilterContracts(filter);

            var sb = new StringBuilder();

            // Header CSV
            sb.AppendLine("ID,Customer,Agent,Post,Amount,Commission,Status,CreatedAt");

            foreach (var c in contracts)
            {
                sb.AppendLine(
                    $"{c.id}," +
                    $"\"{c.json_customer?.full_name}\"," +
                    $"\"{c.json_agent?.full_name}\"," +
                    $"\"{c.json_post?.title}\"," +
                    $"{c.amount}," +
                    $"{c.commission}," +
                    $"{c.status}," +
                    $"{c.created_at:yyyy-MM-dd}"
                );
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
