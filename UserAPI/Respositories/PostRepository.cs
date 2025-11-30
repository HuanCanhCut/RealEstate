using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.DTO.Request;
using UserAPI.Helpers;
using UserAPI.Models;
using UserAPI.Repositories;
using UserAPI.Respositories.Interfaces;

namespace UserAPI.Respositories
{
    public class PostRepository(DbContext dbContext) : IPostRepository
    {
        private readonly DbContext _dbContext = dbContext;

        public int CreatePost(PostRequest post)
        {
            try
            {
                string postInsertSql = @$"
                    INSERT INTO
                        posts (
                            title,
                            description,
                            address,
                            administrative_address,
                            project_type,
                            images,
                            category_id,
                            user_id,
                            role
                        )
                    VALUES (
                            '{post.title}',
                            '{post.description}',
                            '{post.address}',
                            '{post.administrative_address}',
                            '{post.project_type}',
                            '{post.images}',
                            {post.category_id},
                            {post.user_id},
                            'user'
                        );
                    
                    SELECT LAST_INSERT_ID();        
                ";

                int postId = Convert.ToInt32(_dbContext.ExecuteScalar(postInsertSql));


                string postDetailInsertSql = @$"
                    INSERT INTO
                        post_details (
                            post_id,
                            bedrooms,
                            bathrooms,
                            balcony,
                            main_door,
                            legal_documents,
                            interior_status,
                            area,
                            price,
                            deposit
                        )
                    VALUES (
                            {postId},
                            {post.details.bedrooms},
                            {post.details.bathrooms},
                            '{post.details.balcony}',
                            '{post.details.main_door}',
                            '{post.details.legal_documents}',
                            '{post.details.interior_status}',
                            {post.details.area},
                            {post.details.price},
                            {post.details.deposit}
                        );
                ";

                Console.WriteLine(postDetailInsertSql);

                _dbContext.ExecuteNonQuery(postDetailInsertSql);

                return postId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PostModel? GetPostById(int id)
        {
            try
            {
                string sql = $@"
                    SELECT
                        *
                    FROM
                        posts
                        JOIN post_details ON posts.id = post_details.post_id
                    WHERE
                        posts.id = {id};
                ";

                DataTable table = _dbContext.ExecuteQuery(sql);

                return table.MapWithSingleChild<PostModel, PostDetailModel>(
                    "id",
                    row => table.ConvertTo<PostModel>()?.FirstOrDefault(),
                    row => table.ConvertTo<PostDetailModel>()?.FirstOrDefault(),
                    (parent, children) => parent.post_details = children
                ).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}