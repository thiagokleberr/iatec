using Sales.Enums;
using Sales.Models;
using Sales.Services.Interfaces;
using Sales.ViewModel;

namespace Sales.Services;

public class SalesService : ISalesService
{
    private readonly List<Sale> _sales = new List<Sale>();
    private static int _nextSaleId = 1;
    private static int _nextOrderId = 1;

    public Sale? GetSale(int id)
    {
        return _sales.FirstOrDefault(x => x.Id == id);
    }

    public Sale RegisterSale(SaleRequest saleRequest)
    {
        var sale = new Sale()
        {
            Id = _nextSaleId++,
            Status = ESaleStatus.AwaitingPayment,
            Date = DateTime.Now,
            OrderId = _nextOrderId++,
            Seller = saleRequest.Seller,
            Items = saleRequest.Items
        };

        _sales.Add(sale);

        return sale;
    }

    public bool UpdateSaleStatus(int id, ESaleStatus newStatus)
    {
        var sale = _sales.FirstOrDefault(x => x.Id == id);

        if (sale == null)
            throw new KeyNotFoundException("Venda não encontrada");

        if (!IsValidStatusTransition(sale.Status, newStatus))
            throw new InvalidOperationException("Transição de status inválida");

        sale.Status = newStatus;

        return true;
    }

    private bool IsValidStatusTransition(ESaleStatus currentStatus, ESaleStatus newStatus)
    {
        return (currentStatus == ESaleStatus.AwaitingPayment &&
                (newStatus == ESaleStatus.PaymentApproved || newStatus == ESaleStatus.Cancelled)) ||
               (currentStatus == ESaleStatus.PaymentApproved &&
                (newStatus == ESaleStatus.Shipped || newStatus == ESaleStatus.Cancelled)) ||
               (currentStatus == ESaleStatus.Shipped && newStatus == ESaleStatus.Delivered);
    }
}