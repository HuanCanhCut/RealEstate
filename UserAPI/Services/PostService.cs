using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.DTO.Request;
using UserAPI.Models;
using UserAPI.Respositories;
using UserAPI.Respositories.Interfaces;
using UserAPI.Services.Interfaces;
using static UserAPI.Errors.Error;

namespace UserAPI.Services
{
    public class PostService(IPostRepository postRepository) : IPostService
    {
        private readonly IPostRepository _postRepository = postRepository;

        public PostModel CreatePost(PostRequest post)
        {
            try
            {
                int postId = _postRepository.CreatePost(post);

                PostModel postData = this.GetPostById(postId);

                return postData;
            }
            catch (Exception ex)
            {
                if (ex is AppError)
                {
                    throw;
                }

                throw new InternalServerError(ex.Message, new
                {
                    stack_trace = ex.StackTrace
                });
            }
        }

        public PostModel GetPostById(int id)
        {
            try
            {
                return _postRepository.GetPostById(id);
            }
            catch (Exception ex)
            {
                if (ex is AppError)
                {
                    throw;
                }

                throw new InternalServerError(ex.Message, new
                {
                    stack_trace = ex.StackTrace
                });
            }
        }
    }
}