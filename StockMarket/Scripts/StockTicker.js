if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}


$(function () {

            var stockHub = $.connection.stockHub;

            $stockTable = $('#stockTable');
            $stockTableBody = $stockTable.find('tbody');

            rowTemplate = '<tr data-symbol="{Name}"><td>{Name}</td><td>{CurrentPrice}</td><td>{Change}</td><td>{PercentageChange}</td></tr>';

            $.connection.hub.start().done(init);

            function formatStock(stock) {
                return $.extend(stock, {
                    Name : stock.Name, 
                    CurrentPrice: stock.CurrentPrice.toFixed(2),
                    Change: stock.Change.toFixed(2),
                    PercentageChange: (stock.PercentageChange).toFixed(2) + '%',
                });
            }

            function init() {
                stockHub.server.getAllStocks().done(function (stocks) {
                    $stockTableBody.empty();
                    $.each(stocks, function () {
                        var stock = formatStock(this);
                        $stockTableBody.append(rowTemplate.supplant(stock));
                    });
                });
            };

            stockHub.client.changeStockPrice = function (stock) {
                var displayStock = formatStock(stock),
                    $row = $(rowTemplate.supplant(displayStock));
                $stockTableBody.find('tr[data-symbol=' + stock.Name + ']')
                    .replaceWith($row);
            }; 

        });

