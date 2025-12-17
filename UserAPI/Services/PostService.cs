using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.DTO.Request;
using UserAPI.DTO.ServiceResponse;
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

        public PostModel CreatePost(CreatePostRequest post)
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

        public GetPostServiceResponse GetPosts(GetPostRequest request)
        {
            try
            {
                List<PostModel>? result = _postRepository.GetPosts(request);
                int total = _postRepository.CountAll();

                return new GetPostServiceResponse
                {
                    data = result,
                    total = total,
                    count = result.Count
                };
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

        public List<PostModel> SearchPosts(string q)
        {
            try
            {
                List<PostModel>? posts = _postRepository.SearchPosts(q);

                return posts;
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

        public PostModel LikePost(int post_id, int user_id)
        {
            try
            {
                PostModel post = _postRepository.GetPostById(post_id);

                if (post == null)
                {
                    throw new NotFoundError("Bài đăng không tồn tại");
                }

                if (post.is_favorite)
                {
                    throw new BadRequestError("Bài đăng đã được thích");
                }

                post.is_favorite = !post.is_favorite;

                int result = _postRepository.LikePost(post_id: post_id, user_id: user_id);

                if (result <= 0)
                {
                    throw new InternalServerError("Lỗi khi thích bài đăng");
                }

                return post;
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

        public void UnlikePost(int post_id, int user_id)
        {
            try
            {
                PostModel post = _postRepository.GetPostById(post_id);

                if (post == null)
                {
                    throw new NotFoundError("Bài đăng không tồn tại");
                }

                if (!post.is_favorite)
                {
                    throw new BadRequestError("Bài đăng đã không được thích");
                }

                int result = _postRepository.UnlikePost(post_id: post_id, user_id: user_id);

                if (result <= 0)
                {
                    throw new InternalServerError("Lỗi khi bỏ thích bài đăng");
                }

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