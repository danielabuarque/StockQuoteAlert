using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockQuoteAlert.Dtos
{
    public class ResultDto
    {
        public decimal RegularMarketPrice { get; set; }
        public string Symbol { get; set; } = string.Empty;
    }
}
