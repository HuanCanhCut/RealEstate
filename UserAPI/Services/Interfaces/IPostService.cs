using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.DTO.Request;
using UserAPI.DTO.Response;
using UserAPI.DTO.ServiceResponse;
using UserAPI.Models;

namespace UserAPI.Services.Interfaces
{
    public interface IPostService
    {
        PostModel CreatePost(CreatePostRequest post);
        GetPostServiceResponse GetPosts(GetPostRequest request, int currentUserId);
        ServiceResponsePagination<PostModel> SearchPosts(string q, int page, int per_page);
        PostModel LikePost(int post_id, int user_id);
        void UnlikePost(int post_id, int user_id);
        PostModel UpdatePost(int id, UpdatePostRequest request, int currentUserId);

        void DeletePost(int post_id, int user_id);
        PostModel GetPostById(int id, int currentUserId, bool force = false);
    }
}