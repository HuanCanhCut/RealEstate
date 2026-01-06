using AdminAPI.DTO.Response;
using AdminAPI.Models;
using AdminAPI.Models.Enums;
using AdminAPI.Repositories.Interfaces;
using AdminAPI.Services.Interfaces;
using static AdminAPI.Errors.Error;

namespace AdminAPI.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public PostModel GetPostById(int postId)
        {
            try
            {
                PostModel post = _postRepository.GetPostById(postId);

                if (post == null)
                {
                    throw new NotFoundError("Không tìm thấy bài đăng.");
                }

                return post;
            }
            catch (Exception ex)
            {
                if (ex is AppError) { throw; }

                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }

        public int UpdatePostStatus(int postId, PostEnum type)
        {
            try
            {
                PostModel post = this.GetPostById(postId);

                switch (type)
                {
                    case PostEnum.approved:
                        if (post.post_status == PostEnum.approved)
                        {
                            throw new BadRequestError("Bài đăng đã được phê duyệt trước đó.");
                        }
                        break;
                    case PostEnum.rejected:
                        if (post.post_status == PostEnum.rejected)
                        {
                            throw new BadRequestError("Bài đăng đã bị từ chối trước đó.");
                        }
                        break;
                    case PostEnum.pending:
                        if (post.post_status == PostEnum.pending)
                        {
                            throw new BadRequestError("Bài đăng đang ở trạng thái chờ xử lý.");
                        }
                        break;
                    default:
                        throw new BadRequestError("Loại trạng thái bài đăng không hợp lệ.");
                }

                int rowAffected = _postRepository.UpdatePostStatus(postId, type);
                if (rowAffected == 0)
                {
                    throw new InternalServerError("Cập nhật bài đăng thất bại.");
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                if (ex is AppError) { throw; }

                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }

        public int CountAll()
        {
            try
            {
                return _postRepository.CountAll();
            }
            catch (Exception ex)
            {
                if (ex is AppError) { throw; }

                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }

        public void DeletePost(int post_id)
        {
            try
            {
                PostModel post = this.GetPostById(post_id);
                if (post == null)
                {
                    throw new NotFoundError("Không tìm thấy bài đăng.");
                }
                int rowAffected = _postRepository.DeletePost(post_id);

                if (rowAffected == 0)
                {
                    throw new InternalServerError("Xoá bài đăng thất bại.");
                }
            }
            catch (Exception ex)
            {
                if (ex is AppError) { throw; }
                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }

        public ServiceResponsePagination<PostModel> GetPosts(int page, int per_page, PostEnum? post_status, ProjectType? project_type, int? category_id)
        {
            try
            {
                // return _postRepository.GetPosts(page, per_page, post_status, project_type, category_id);
                int total = this.CountAll();
                List<PostModel> posts = _postRepository.GetPosts(page, per_page, post_status, project_type, category_id);

                return new ServiceResponsePagination<PostModel>
                {
                    data = posts,
                    total = total,
                    count = posts.Count
                };
            }
            catch (Exception ex)
            {
                if (ex is AppError)
                {
                    throw;
                }
                throw new InternalServerError(ex.Message + ex.StackTrace);
            }
        }
    }
}
