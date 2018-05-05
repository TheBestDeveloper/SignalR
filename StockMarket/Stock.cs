using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockMarket
{
    public class Stock
    {
        public string Name { get; set; }

        public double CurrentPrice { get; set; }

        public double OpenPrice { get; set; }

        public double Change {  get
            {
                return CurrentPrice - OpenPrice; 
            } }

        public double PercentageChange
        {
            get
            {
                return (Change / OpenPrice) * 100; 
            }
        }
    }
}