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
        IList<PostModel> GetPosts(GetPostRequest request);
        int Count();
    }
}