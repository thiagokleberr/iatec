using Sales.Enums;
using Sales.Models;
using Sales.ViewModel;

namespace Sales.Services.Interfaces;

public interface ISalesService
{
    Sale RegisterSale(SaleRequest sale);
    Sale? GetSale(int id);
    bool UpdateSaleStatus(int id, ESaleStatus newStatus);
}
