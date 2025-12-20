using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models.Interfaces;

namespace UserAPI.Models
{
    public class BlacklistedTokenModel : DateInterface
    {
        public int id { get; set; }
        public required string token { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}