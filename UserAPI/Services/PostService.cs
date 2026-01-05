using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.DTO.Request;
using UserAPI.DTO.Response;
using UserAPI.DTO.ServiceResponse;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
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

                PostModel postData = this.GetPostById(postId, 0, true);

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

        public PostModel GetPostById(int id, int currentUserId, bool force = false) // force = true: lấy bài đăng dù đã bị xóa, chưa duyệt, chưa bàn giao, ...
        {
            try
            {
                return _postRepository.GetPostById(id, currentUserId, force);
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

        public GetPostServiceResponse GetPosts(GetPostRequest request, int currentUserId)
        {
            try
            {
                List<PostModel>? result = _postRepository.GetPosts(request, currentUserId);
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

        public ServiceResponsePagination<PostModel> SearchPosts(string q, int page, int per_page)
        {
            try
            {
                List<PostModel>? posts = _postRepository.SearchPosts(q, page, per_page);

                int total = _postRepository.CountAll();

                return new ServiceResponsePagination<PostModel>
                {
                    count = posts.Count(),
                    total = total,
                    data = posts,
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

        public PostModel LikePost(int post_id, int user_id)
        {
            try
            {
                PostModel post = _postRepository.GetPostById(post_id, user_id);

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
                PostModel post = _postRepository.GetPostById(post_id, user_id);

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

        public PostModel UpdatePost(int id, UpdatePostRequest request, int currentUserId)
        {
            try
            {
                PostModel post = _postRepository.GetPostById(id, currentUserId);

                if (post == null)
                {
                    throw new NotFoundError("Bài đăng không tồn tại");
                }

                int rowAffected = _postRepository.UpdatePost(id, request);

                if (rowAffected <= 0)
                {
                    throw new InternalServerError("Lỗi khi cập nhật bài đăng");
                }

                return this.GetPostById(id, currentUserId);
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

        public void DeletePost(int post_id, int user_id)
        {
            try
            {
                PostModel post = _postRepository.GetPostById(post_id, user_id);

                if (post == null)
                {
                    throw new NotFoundError("Bài đăng không tồn tại");
                }

                if (post.json_user?.id != user_id)
                {
                    throw new ForbiddenError("Bạn không có quyền xóa bài đăng này");
                }

                int rowAffected = _postRepository.DeletePost(post_id, user_id);

                if (rowAffected <= 0)
                {
                    throw new InternalServerError("Lỗi khi xóa bài đăng");
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