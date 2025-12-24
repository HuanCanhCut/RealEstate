using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAPI.DTO.Request
{
    public class CreateCategoryRequest
    {
        public required string name { get; set; }
        public required string key { get; set; }
    }
}