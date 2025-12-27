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

        public int ApprovePost(int postId)
        {
            try
            {
                PostModel post = this.GetPostById(postId);

                if (post.post_status == PostEnum.approved)
                {
                    throw new BadRequestError("Bài đăng đã được phê duyệt trước đó.");
                }

                int rowAffected = _postRepository.ApprovePost(postId);
                if (rowAffected == 0)
                {
                    throw new InternalServerError("Phê duyệt bài đăng thất bại.");
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
    }
}
