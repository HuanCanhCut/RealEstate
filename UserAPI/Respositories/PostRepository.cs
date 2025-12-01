using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using UserAPI.DTO.Request;
using UserAPI.DTO.ServiceResponse;
using UserAPI.Models;
using UserAPI.Repositories;
using UserAPI.Respositories.Interfaces;

namespace UserAPI.Respositories
{
    public class PostRepository(DbContext dbContext) : IPostRepository
    {
        private readonly DbContext _dbContext = dbContext;

        public int CreatePost(CreatePostRequest post)
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
                        *,
                        (
                            SELECT 
                                JSON_ARRAYAGG(
                                    JSON_OBJECT(
                                        'id', id,
                                        'bedrooms', bedrooms,
                                        'bathrooms', bathrooms,
                                        'balcony', balcony,
                                        'main_door', main_door,
                                        'legal_documents', legal_documents,
                                        'interior_status', interior_status,
                                        'area', area,
                                        'price', price,
                                        'deposit', deposit,
                                        'post_id', post_id
                                    )
                                )
                            FROM post_details
                            WHERE post_details.post_id = posts.id
                        ) AS json_post_detail
                    FROM
                        posts
                    WHERE
                        posts.id = {id};
                ";

                DataTable table = _dbContext.ExecuteQuery(sql);

                return table.ConvertTo<PostModel>()?.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        public IList<PostModel>? GetPosts(GetPostRequest request)
        {
            try
            {
                string sql = @$"
                    SELECT 
                        *,
                        (
                            SELECT 
                                    JSON_OBJECT(
                                        'bedrooms', bedrooms,
                                        'bathrooms', bathrooms,
                                        'balcony', balcony,
                                        'main_door', main_door,
                                        'legal_documents', legal_documents,
                                        'interior_status', interior_status,
                                        'area', area,
                                        'price', price,
                                        'deposit', deposit,
                                        'post_id', post_id
                                    )
                            FROM post_details
                            WHERE post_details.post_id = posts.id
                        ) AS json_post_detail,
                        (
                            SELECT JSON_OBJECT(
                                'id', id,
                                'name', name,
                                'created_at', created_at,
                                'updated_at', updated_at
                            )
                            FROM categories
                            WHERE categories.id = posts.category_id
                        ) AS json_category
                    FROM posts
                    JOIN categories ON categories.id = posts.category_id 
                    JOIN post_details ON post_details.post_id = posts.id
                    WHERE 1=1 
                ";

                if (!String.IsNullOrEmpty(request.property_categories?.Length.ToString()))
                {
                    sql += $" AND categories.name IN ({string.Join(", ", request.property_categories.Select(x => $"'{x}'"))})";
                }

                if (request.project_type != null)
                {
                    sql += $" AND project_type = '{request.project_type}'";
                }

                if (request.min_price != null)
                {
                    sql += $" AND post_details.price >= {request.min_price}";
                }

                if (request.max_price != null)
                {
                    sql += $" AND post_details.price <= {request.max_price}";
                }

                sql += $" LIMIT {request.per_page} OFFSET {(request.page - 1) * request.per_page}";

                DataTable table = _dbContext.ExecuteQuery(sql);

                return table.ConvertTo<PostModel>();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Count()
        {
            try
            {
                string sql = "SELECT COUNT(1) FROM posts";

                return Convert.ToInt32(_dbContext.ExecuteScalar(sql));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}