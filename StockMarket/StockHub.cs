using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace StockMarket
{
    public class StockHub : Hub
    {

        private readonly StockTicker stockTicker;

        public StockHub()
        {
            stockTicker = StockTicker.Instance; 
        }

        public IEnumerable<Stock> GetAllStocks()
        {
            return stockTicker.GetAllStocks(); 
        }
    }
}