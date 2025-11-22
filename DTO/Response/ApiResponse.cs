using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.DTO.Response
{
    public class ApiResponse<T, M>(T data, M? meta = default)
    {
        public T Data { get; set; } = data;

        public M? Meta { get; set; } = meta;
    }
}