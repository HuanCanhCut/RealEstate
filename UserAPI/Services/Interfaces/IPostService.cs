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
        PostModel GetPostById(int id);
    }
}