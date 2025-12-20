using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.DTO.Request;
using UserAPI.DTO.ServiceResponse;
using UserAPI.Models;

namespace UserAPI.Services.Interfaces
{
        public interface IPostService
        {
                PostModel CreatePost(CreatePostRequest post);
                GetPostServiceResponse GetPosts(GetPostRequest request);
                List<PostModel> SearchPosts(string q);
                PostModel LikePost(int post_id, int user_id);
                void UnlikePost(int post_id, int user_id);
                PostModel UpdatePost(int id, UpdatePostRequest request);

                void DeletePost(int post_id, int user_id);
                PostModel GetPostById(int id);
        }
}