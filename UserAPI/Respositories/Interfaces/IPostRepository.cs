using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.DTO.Request;
using UserAPI.DTO.ServiceResponse;
using UserAPI.Models;

namespace UserAPI.Respositories.Interfaces
{
    public interface IPostRepository
    {
        int CreatePost(CreatePostRequest post);
        PostModel GetPostById(int id);
        List<PostModel> GetPosts(GetPostRequest request);
        int CountAll();
        List<PostModel> SearchPosts(string q);
        int LikePost(int post_id, int user_id);

        int UnlikePost(int post_id, int user_id);
    }
}