using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.DTO.Request;
using AdminAPI.DTO.Response;
using AdminAPI.Middlewares;
using AdminAPI.Models;
using AdminAPI.Services.Interfaces;
using AdminAPI.Utils;
using AdminAPI.Utils.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminAPI.Controllers
{
    [VerifyToken]
    [VerifyAdmin]
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController(
       ICategoryService categoryService,
       IJWT jwt
       ) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;
        private readonly IJWT _jwt = jwt;

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<ApiResponse<List<CategoryModel>, object?>> GetCategories()
        {
            try
            {
                JwtDecoded? decoded = null;

                string? token = HttpContext.Request.Cookies["access_token"];
                if (!string.IsNullOrEmpty(token))
                {
                    decoded = _jwt.ValidateToken(
                        token,
                        Environment.GetEnvironmentVariable("JWT_SECRET_KEY")!
                    );
                }

                List<CategoryModel> response = _categoryService.GetCategories(decoded?.sub);
                return Ok(new ApiResponse<List<CategoryModel>, object?>(response));
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult<ApiResponse<CategoryModel, object?>> CreateCategory(CreateCategoryRequest request)
        {
            try
            {

                JwtDecoded decoded = (JwtDecoded)HttpContext.Items["decoded"]!;

                CategoryModel category = _categoryService.CreateCategory(request.name, request.key);

                return CreatedAtAction(nameof(CreateCategory), new ApiResponse<CategoryModel, object?>(category));
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("{id}")]
        public ActionResult<ApiResponse<CategoryModel, object?>> UpdateCategory(int id, UpdateCategoryRequest request)
        {
            try
            {
                JwtDecoded decoded = (JwtDecoded)HttpContext.Items["decoded"]!;

                CategoryModel category = _categoryService.UpdateCategory(id, request.name, request.key);

                return Ok(new ApiResponse<CategoryModel, object?>(category));
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<object, object?>> DeleteCategory(int id)
        {
            try
            {
                _categoryService.DeleteCategory(id);

                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}