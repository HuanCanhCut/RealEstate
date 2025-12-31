using AdminAPI.DTO.Request;
using AdminAPI.DTO.Response;
using AdminAPI.Middlewares;
using AdminAPI.Models;
using AdminAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminAPI.Controllers
{
    [VerifyToken]
    [VerifyAdmin]
    [Route("api/contracts")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractSevice _contractService;
        public ContractController(IContractSevice contractService)
        {
            _contractService = contractService;
        }

        [HttpGet]
        public ActionResult<ApiResponse<List<ContractModel>, MetaPagination>> GetContracts([FromQuery] PaginationRequest query)
        {
          try
                {
                ServiceResponsePagination<ContractModel> serviceResponse = _contractService.GetContracts(query.page, query.per_page);

                return Ok(new ApiResponse<List<ContractModel>, MetaPagination> (
                    data: serviceResponse.data,
                    meta: new MetaPagination(
                            total: serviceResponse.total,
                            count: serviceResponse.count,
                            current_page: query.page,
                            per_page: query.per_page
                        )
                    ));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ApiResponse<ContractModel, object>> GetContractDetail(int id)
        {
            try
            {
                ContractModel contract = _contractService.GetContractById(id);

                return Ok(new ApiResponse<ContractModel, object>(
                    data: contract,
                    meta: null
                ));
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
