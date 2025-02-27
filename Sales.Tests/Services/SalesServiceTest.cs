using Moq;
using Sales.Enums;
using Sales.Models;
using Sales.Services;
using Sales.ViewModel;

namespace Sales.Tests.Services;


public class SalesServiceTests
{

    private readonly SalesService _salesService;

    public SalesServiceTests()
    {
        _salesService = new SalesService();
    }

    [Fact]
    public void RegisterSale_ShouldAddSale()
    {
        // Arrange
        var saleRequest = new SaleRequest
        {
            Seller = new Seller { Id = 1, Cpf = "12345678900", Name = "Thiago Kleber" },
            Items = new List<Item> { new Item { Description = "Camiseta", Price = 1.99m } }
        };

        // Act
        var sale = _salesService.RegisterSale(saleRequest);

        // Assert
        Assert.NotNull(sale);
        Assert.Equal(ESaleStatus.AwaitingPayment, sale.Status);
        Assert.Equal(saleRequest.Seller, sale.Seller);
        Assert.Equal(saleRequest.Items, sale.Items);
    }

    [Fact]
    public void GetSale_ShouldReturnSale_WhenSaleExists()
    {
        // Arrange
        var saleRequest = new SaleRequest
        {
            Seller = new Seller { Id = 1, Cpf = "12345678900", Name = "Thiago Kleber" },
            Items = new List<Item> { new Item { Description = "Camiseta", Price = 1.99m } }
        };
        var sale = _salesService.RegisterSale(saleRequest);

        // Act
        var retrievedSale = _salesService.GetSale(sale.Id);

        // Assert
        Assert.NotNull(retrievedSale);
        Assert.Equal(sale.Id, retrievedSale.Id);
    }

    [Fact]
    public void UpdateSaleStatus_ShouldUpdateStatus_WhenTransitionIsValid()
    {
        // Arrange
        var saleRequest = new SaleRequest
        {
            Seller = new Seller { Id = 1, Cpf = "12345678900", Name = "Thiago Kleber" },
            Items = new List<Item> { new Item { Description = "Camiseta", Price = 1.99m } }
        };
        var sale = _salesService.RegisterSale(saleRequest);

        // Act
        var result = _salesService.UpdateSaleStatus(sale.Id, ESaleStatus.PaymentApproved);

        // Assert
        Assert.True(result);
        Assert.Equal(ESaleStatus.PaymentApproved, sale.Status);
    }

    [Fact]
    public void UpdateSaleStatus_ShouldThrowException_WhenTransitionIsInvalid()
    {
        // Arrange
        var saleRequest = new SaleRequest
        {
            Seller = new Seller { Id = 1, Cpf = "12345678900", Name = "Thiago Kleber" },
            Items = new List<Item> { new Item { Description = "Camiseta", Price = 1.99m } }
        };
        var sale = _salesService.RegisterSale(saleRequest);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _salesService.UpdateSaleStatus(sale.Id, ESaleStatus.Delivered));
    }

    [Fact]
    public void UpdateSaleStatus_ShouldThrowException_WhenSaleWasNotFound()
    {
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _salesService.UpdateSaleStatus(It.IsAny<int>(), ESaleStatus.Delivered));
    }

}
