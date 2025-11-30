using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.DTO.Request;
using UserAPI.Models;

namespace UserAPI.Respositories.Interfaces
{
    public interface IPostRepository
    {
        int CreatePost(PostRequest post);
        PostModel GetPostById(int id);
    }
}