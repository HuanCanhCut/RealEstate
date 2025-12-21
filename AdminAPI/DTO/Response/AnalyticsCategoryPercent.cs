using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminAPI.Models;

namespace AdminAPI.DTO.Response
{
    public class AnalyticsCategoryPercent : CategoryModel
    {
        public required decimal percentage { get; set; }
    }
}