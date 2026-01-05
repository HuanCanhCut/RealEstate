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
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Repositories
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

        public PostModel? GetPostById(int id, int currentUserId, bool force = false)
        {
            try
            {
                string sql = $@"
                    SELECT
                        *,
                        JSON_OBJECT(
                            'id', post_details.id,
                            'type', post_details.type,
                            'area', post_details.area,
                            'price', post_details.price,
                            'post_id', post_details.post_id,
                            'bedrooms', post_details.bedrooms,
                            'bathrooms', post_details.bathrooms,
                            'balcony', post_details.balcony,
                            'main_door', post_details.main_door,
                            'legal_documents', post_details.legal_documents,
                            'interior_status', post_details.interior_status,
                            'deposit', post_details.deposit,
                            'created_at', post_details.created_at,
                            'updated_at', post_details.updated_at
                        ) AS json_post_detail,

                        JSON_OBJECT(
                            'id', categories.id,
                            'name', categories.name,
                            'key', categories.key,
                            'created_at', categories.created_at,
                            'updated_at', categories.updated_at
                        ) AS json_category,

                        JSON_OBJECT(
                            'id', users.id,
                            'full_name', users.full_name,
                            'avatar', users.avatar,
                            'phone_number', users.phone_number,
                            'role', users.role,
                            'post_count', user_post_count.post_count,
                            'nickname', users.nickname
                        ) AS json_user,
                        (
                            SELECT EXISTS (
                                SELECT 1
                                FROM favorites
                                WHERE favorites.user_id = {currentUserId}
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

                    WHERE posts.id = {id}
                ";

                if (!force)
                {
                    sql += $@"
                        AND posts.post_status = 'approved'
                        AND posts.status = 'Chưa bàn giao'
                        AND posts.is_deleted = 0
                        AND posts.deleted_at IS NULL
                    ";
                }

                DataTable table = _dbContext.ExecuteQuery(sql);

                return table.ConvertTo<PostModel>()?.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PostModel>? GetPosts(GetPostRequest request, int currentUserId)
        {
            try
            {
                string sql = @$"
                    SELECT
                        *,

                        JSON_OBJECT(
                            'id', post_details.id,
                            'type', post_details.type,
                            'area', post_details.area,
                            'price', post_details.price,
                            'post_id', post_details.post_id,
                            'bedrooms', post_details.bedrooms,
                            'bathrooms', post_details.bathrooms,
                            'balcony', post_details.balcony,
                            'main_door', post_details.main_door,
                            'legal_documents', post_details.legal_documents,
                            'interior_status', post_details.interior_status,
                            'deposit', post_details.deposit,
                            'created_at', post_details.created_at,
                            'updated_at', post_details.updated_at
                        ) AS json_post_detail,

                        JSON_OBJECT(
                            'id', categories.id,
                            'name', categories.name,
                            'key', categories.key,
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
                                WHERE favorites.user_id = {currentUserId}
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

                    WHERE posts.post_status = 'approved'
                    AND posts.status = 'Chưa bàn giao'
                    AND posts.is_deleted = 0
                    AND posts.deleted_at IS NULL
                    AND 1=1
                ";

                if (!String.IsNullOrEmpty(request.property_categories?.Length.ToString()))
                {
                    sql += $" AND categories.key IN ({string.Join(", ", request.property_categories.Select(x => $"'{x}'"))})";
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

                if (request.role != Role.all)
                {
                    sql += $" AND posts.role = '{request.role}'";
                }

                if (request.location != null && request.location != "all")
                {
                    sql += $" AND administrative_address LIKE '%{request.location}%'";
                }

                sql += $" ORDER BY posts.id DESC LIMIT {request.per_page} OFFSET {(request.page - 1) * request.per_page}";

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

        public List<PostModel> SearchPosts(string q, int page, int per_page)
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
                    LIMIT {per_page} OFFSET {(page - 1) * per_page}
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

        public int UpdatePost(int id, UpdatePostRequest request)
        {
            try
            {
                string sql = $@"
                    UPDATE 
                        posts 
                    SET 
                        title = '{request.title}',
                        description = '{request.description}',
                        address = '{request.address}',
                        administrative_address = '{request.administrative_address}',
                        project_type = '{request.project_type}',
                        images = '{request.images}',
                        category_id = {request.category_id},
                        user_id = {request.user_id},
                        role = 'user' 
                    WHERE id = {id};

                    UPDATE 
                        post_details 
                    SET 
                        bedrooms = {request.details.bedrooms},
                        bathrooms = {request.details.bathrooms},
                        balcony = '{request.details.balcony}',
                        main_door = '{request.details.main_door}',
                        legal_documents = '{request.details.legal_documents}',
                        interior_status = '{request.details.interior_status}',
                        area = {request.details.area},
                        price = {request.details.price},
                        deposit = {request.details.deposit}
                    WHERE post_id = {id};
                ";

                return _dbContext.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int DeletePost(int post_id, int user_id)
        {
            try
            {
                string sql = $@"
                    DELETE FROM posts WHERE id = {post_id} AND user_id = {user_id};
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