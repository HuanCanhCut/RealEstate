using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAPI.Models;

namespace UserAPI.DTO.ServiceResponse
{
        public class GetPostServiceResponse
        {
                public required List<PostModel> data { get; set; }

                public required int total { get; set; }
                public required int count { get; set; }
        }
}