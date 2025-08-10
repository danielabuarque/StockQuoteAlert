using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockQuoteAlert.Dtos
{
    public class StockApiResponseDto
    {
        public List<ResultDto>? Results { get; set; }
    }
}
