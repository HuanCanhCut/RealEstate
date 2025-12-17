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
                        ) AS json_post_detail,
                        (
                            SELECT EXISTS (
                                SELECT 1
                                FROM favorites
                                WHERE favorites.user_id = users.id
                                AND favorites.post_id = posts.id
                            )
                        ) AS is_favorite
                    FROM
                        posts
                    JOIN users ON users.id = posts.user_id
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

        public List<PostModel>? GetPosts(GetPostRequest request)
        {
            try
            {
                string sql = @$"
                    SELECT
                        *,

                        JSON_OBJECT(
                            'area', post_details.area,
                            'price', post_details.price,
                            'post_id', post_details.post_id
                        ) AS json_post_detail,

                        JSON_OBJECT(
                            'id', categories.id,
                            'name', categories.name,
                            'created_at', categories.created_at,
                            'updated_at', categories.updated_at
                        ) AS json_category,

                        JSON_OBJECT(
                            'id', users.id,
                            'full_name', users.full_name,
                            'avatar', users.avatar,
                            'nickname', users.nickname,
                            'post_count', user_post_count.post_count
                        ) AS json_user,

                        (
                            SELECT EXISTS (
                                SELECT 1
                                FROM favorites
                                WHERE favorites.user_id = users.id
                                AND favorites.post_id = posts.id
                            )
                        ) AS is_favorite
                    FROM posts
                    JOIN post_details ON post_details.post_id = posts.id
                    JOIN categories ON categories.id = posts.category_id
                    JOIN users ON users.id = posts.user_id

                    LEFT JOIN (
                        SELECT user_id, COUNT(*) AS post_count
                        FROM posts
                        GROUP BY user_id
                    ) user_post_count ON user_post_count.user_id = users.id

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

                foreach (DataRow row in table.Rows)
                {
                    if (row["is_favorite"] != DBNull.Value)
                    {
                        long value = Convert.ToInt64(row["is_favorite"]);
                        row["is_favorite"] = value != 0;
                    }
                }

                return table.ConvertTo<PostModel>();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CountAll()
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

        public List<PostModel> SearchPosts(string q)
        {
            try
            {
                string sql = $@"
                    SELECT 
                        *,
                        (
                            SELECT 
                                JSON_OBJECT(
                                    'area', area,
                                    'price', price,
                                    'post_id', post_id
                                )
                            FROM post_details
                            WHERE post_details.post_id = posts.id
                        ) AS json_post_detail
                    FROM
                        posts
                    WHERE
                        MATCH(title, address, administrative_address) AGAINST ('{q}*' IN BOOLEAN MODE)
                    LIMIT 5;
                ";

                DataTable table = _dbContext.ExecuteQuery(sql);

                return table.ConvertTo<PostModel>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int LikePost(int post_id, int user_id)
        {
            try
            {
                string sql = $@"
                    INSERT INTO favorites (user_id, post_id) VALUES ({user_id}, {post_id});
                ";

                return _dbContext.ExecuteNonQuery(sql);
            }

            catch (Exception)
            {
                throw;
            }
        }

        public int UnlikePost(int post_id, int user_id)
        {
            try
            {
                string sql = $@"
                    DELETE FROM favorites WHERE user_id = {user_id} AND post_id = {post_id};
                ";

                return _dbContext.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}