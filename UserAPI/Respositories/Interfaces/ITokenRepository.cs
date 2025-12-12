using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models;

namespace UserAPI.Respositories.Interfaces
{
    public interface ITokenRepository
    {
        int UpdateRefreshToken(int userId, string newToken, string oldToken);
        int InsertRefreshToken(int userId, string newToken);
        RefreshTokenModel? GetRefreshToken(string token);
        int InsertBlacklistToken(string token);
        BlacklistedTokenModel? GetBlacklistToken(string token);

        int RemoveRefreshToken(string token);
    }
}