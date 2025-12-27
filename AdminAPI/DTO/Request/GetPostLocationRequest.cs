using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAPI.DTO.Request
{
    public class GetPostLocationRequest : RangeTimeRequest
    {
        [Range(1, 100, ErrorMessage = "Limit phải lớn hơn 0 và nhỏ hơn 100")]
        public int limit { get; set; } = 10;
    }
}