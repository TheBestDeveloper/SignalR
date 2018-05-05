using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace StockMarket
{
    public class StockTicker
    {
        // Needs to be lazy initialized to threadsafe 
        private static StockTicker stockTicker = new StockTicker(GlobalHost.ConnectionManager.GetHubContext<StockHub>().Clients);

        public IEnumerable<Stock> GetAllStocks()
        {
            return Stocks.Values; 
        }

        public static StockTicker Instance
        {
            get { return stockTicker;  }
        }

        private IHubConnectionContext<dynamic> Clients;

        private Dictionary<string, Stock> Stocks = new Dictionary<string, Stock>();
        private Random random = new Random(); 
        private Timer stockTimer; 
        private readonly object UpdateStockPriceLock = new object(); 
        private StockTicker(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            Stocks.Clear();

            var _stocks = new List<Stock>();

            _stocks.Add(new Stock { Name = "Eicher", CurrentPrice = 30000.00, OpenPrice = 25000 });
            _stocks.Add(new Stock { Name = "PNB", CurrentPrice = 90, OpenPrice = 100 });
            _stocks.Add(new Stock { Name = "Hudco", CurrentPrice = 65, OpenPrice = 80 });

            _stocks.ForEach(s => Stocks.Add(s.Name, s));

            stockTimer = new Timer(UpdateStockPrice, null, 1500, 1500); 
        }

        private void BroadcastStockPrice(Stock stock)
        {
            Clients.All.ChangeStockPrice(stock); 
        }

        private void UpdateStockPrice(object p)
        {
            int percentageChange = 10;
            var num = random.Next(4);

            if (num == 0) return;

            switch(num)
            {
                case 1:
                    {
                        ChangeStockPrice("Eicher", percentageChange);
                        break;
                    }
                case 2:
                    {
                        ChangeStockPrice("PNB", percentageChange);
                        break;

                    }
                case 3:
                    {
                        ChangeStockPrice("Hudco", percentageChange);
                        break;
                    }
            }

        }
        private void ChangeStockPrice(string name, int percentChange)
        {
            lock(UpdateStockPriceLock)
            {
                if(Stocks.ContainsKey(name))
                {
                    var oldValue = Stocks[name];

                    oldValue.CurrentPrice = oldValue.CurrentPrice * (100 + percentChange) / 100;

                    Stocks[name] = oldValue;
                    BroadcastStockPrice(oldValue); 
                }
            }
        }

    }
}