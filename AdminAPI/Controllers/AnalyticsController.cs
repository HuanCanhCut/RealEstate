using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.DTO.Request;
using AdminAPI.DTO.Response;
using AdminAPI.Middlewares;
using AdminAPI.Models;
using AdminAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static AdminAPI.Errors.Error;

namespace AdminAPI.Controllers
{
    [VerifyToken]
    [VerifyAdmin]
    [ApiController]
    [Route("api/analytics")]
    public class AnalyticsController(IAnalyticsService analyticsService) : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService = analyticsService;

        [HttpGet("overview")]
        public ActionResult<ApiResponse<AnalyticsOverviewResponse, object?>> GetOverview([FromQuery] OverviewRequest request)
        {
            try
            {

                if (request.end_date < request.start_date)
                {
                    throw new BadRequestError("end date must be greater than or equal to start date");
                }

                AnalyticsOverviewResponse response = _analyticsService.GetOverview(request.start_date, request.end_date);

                return Ok(new ApiResponse<AnalyticsOverviewResponse, object?>(response));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("categories")]
        public ActionResult<ApiResponse<List<CategoryModel>, object?>> GetCategories()
        {
            try
            {
                List<CategoryModel> response = _analyticsService.GetCategories();

                return Ok(new ApiResponse<List<CategoryModel>, object?>(response));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}