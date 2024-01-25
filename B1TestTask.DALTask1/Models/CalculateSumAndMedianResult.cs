using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1TestTask.DALTask1.Models
{    
    public class CalculateSumAndMedianResult
    {
        public int Id { get; set; }
        public decimal? SumOfIntegers { get; set; }
        public decimal? MedianOfDecimals { get; set; }
    }
}
