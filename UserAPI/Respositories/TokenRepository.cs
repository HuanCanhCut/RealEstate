using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models;
using UserAPI.Repositories;
using UserAPI.Respositories.Interfaces;

namespace UserAPI.Respositories
{
    public class TokenRepository(DbContext dbContext) : ITokenRepository
    {
        private readonly DbContext _dbContext = dbContext;

        public int UpdateRefreshToken(int userId, string newToken, string oldToken)
        {
            try
            {
                string query = $"UPDATE refresh_tokens SET token = '{newToken}' WHERE user_id = {userId} AND token = '{oldToken}'";

                return _dbContext.ExecuteNonQuery(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertRefreshToken(int userId, string refreshToken)
        {
            try
            {
                string query = $"INSERT INTO refresh_tokens (user_id, token) VALUES ({userId}, '{refreshToken}')";
                return _dbContext.ExecuteNonQuery(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RefreshTokenModel? GetRefreshToken(string token)
        {
            try
            {
                string query = $"SELECT * FROM refresh_tokens WHERE token = '{token}'";
                return _dbContext.ExecuteQuery(query).ConvertTo<RefreshTokenModel>()?.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertBlacklistToken(string token)
        {
            try
            {
                string query = $"INSERT INTO blacklisted_tokens (token) VALUES ('{token}')";
                return _dbContext.ExecuteNonQuery(query);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public BlacklistedTokenModel? GetBlacklistToken(string token)
        {
            try
            {
                string query = $"SELECT * FROM blacklisted_tokens WHERE token = '{token}'";
                return _dbContext.ExecuteQuery(query).ConvertTo<BlacklistedTokenModel>()?.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int RemoveRefreshToken(string token)
        {
            try
            {
                string query = $"DELETE FROM refresh_tokens WHERE token = '{token}'";
                return _dbContext.ExecuteNonQuery(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}