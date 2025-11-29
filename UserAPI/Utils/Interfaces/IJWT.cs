using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserAPI.Utils.Interfaces
{
    public interface IJWT
    {
        string GenerateToken(int userId);
        JwtDecoded ValidateToken(string token);
    }
}