using Microsoft.AspNetCore.Mvc;
using Moq;
using Sales.Controllers;
using Sales.Enums;
using Sales.Models;
using Sales.Services.Interfaces;
using Sales.ViewModel;

namespace Sales.Tests.Controllers;

public class SalesControllerTests
{
    private readonly Mock<ISalesService> _salesServiceMock;
    private readonly SalesController _controller;

    public SalesControllerTests()
    {
        _salesServiceMock = new Mock<ISalesService>();
        _controller = new SalesController(_salesServiceMock.Object);
    }

    [Fact]
    public void PostAsync_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new SaleRequest 
        { 
            Seller = new Seller
            {
                Id = 1,
                Cpf = "51795429011",
                Name = "Thiago Kleber"
            },
            Items = new List<Item>()
            {
                new Item
                {
                    Description = "Camiseta",
                    Price = 1.99M
                }
            }
        };
        var sale = new Sale { Id = 1 };
        _salesServiceMock.Setup(s => s.RegisterSale(request)).Returns(sale);

        // Act
        var result = _controller.PostAsync(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<Response<string>>(okResult.Value);
        Assert.Contains("Venda registrada com sucesso", response.Data);
    }

    [Fact]
    public void PostAsync_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        _controller.ModelState.AddModelError("SellerName", "O nome do vendedor é obrigatório");

        var request = new SaleRequest
        {
            Seller = null,
            Items = new List<Item>()
            {
                new Item
                {
                    Description = "Camiseta",
                    Price = 1.99M
                }
            }
        };
        // Act
        var result = _controller.PostAsync(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<Response<string>>(badRequestResult.Value);

        Assert.NotNull(response.Errors);
        Assert.Contains("O nome do vendedor é obrigatório", response.Errors);
    }

    [Fact]
    public void PostAsync_WhenExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        var request = new SaleRequest
        {
            Seller = new Seller
            {
                Id = 1,
                Cpf = "51795429011",
                Name = "Thiago Kleber"
            },
            Items = new List<Item>()
            {
                new Item
                {
                    Description = "Camiseta",
                    Price = 1.99M
                }
            }
        };

        _salesServiceMock
            .Setup(s => s.RegisterSale(It.IsAny<SaleRequest>()))
            .Throws(new Exception("Falha interna no servidor"));

        // Act
        var result = _controller.PostAsync(request);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var response = Assert.IsType<Response<string>>(objectResult.Value);
        Assert.Equal("Falha interna no servidor", response.Errors.First());
    }

    [Fact]
    public void GetAsync_ExistingSale_ReturnsOk()
    {
        // Arrange
        var sale = new Sale { Id = 1 };
        _salesServiceMock.Setup(s => s.GetSale(1)).Returns(sale);

        // Act
        var result = _controller.GetAsync(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<Response<Sale>>(okResult.Value);
        Assert.Equal(sale.Id, response.Data.Id);
    }

    [Fact]
    public void GetAsync_NonExistingSale_ReturnsNotFound()
    {
        // Arrange
        _salesServiceMock.Setup(s => s.GetSale(1)).Returns((Sale)null);

        // Act
        var result = _controller.GetAsync(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void GetAsync_WhenExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        var sale = new Sale { Id = 1 };
        _salesServiceMock.Setup(s => s.GetSale(1)).Returns(sale);

        _salesServiceMock
            .Setup(s => s.GetSale(It.IsAny<int>()))
            .Throws(new Exception("Falha interna no servidor"));

        // Act
        var result = _controller.GetAsync(1);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var response = Assert.IsType<Response<string>>(objectResult.Value);
        Assert.Equal("Falha interna no servidor", response.Errors.First());
    }

    [Fact]
    public void PutAsync_ValidUpdate_ReturnsOk()
    {
        // Arrange
        var request = new UpdateSaleStatusRequest { Status = ESaleStatus.PaymentApproved };
        _salesServiceMock.Setup(s => s.UpdateSaleStatus(1, request.Status)).Returns(true);

        // Act
        var result = _controller.PutAsync(1, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<Response<string>>(okResult.Value);
        Assert.Equal("Venda atualizada com sucesso!", response.Data);
    }

    [Fact]
    public void PutAsync_WhenKeyNotFoundException_ReturnsNotFound()
    {
        // Arrange
        _salesServiceMock
            .Setup(s => s.UpdateSaleStatus(It.IsAny<int>(), ESaleStatus.Delivered))
            .Throws(new KeyNotFoundException("Venda não encontrada"));

        // Act
        var result = _controller.PutAsync(1, new UpdateSaleStatusRequest { Status = ESaleStatus.Delivered });

        // Assert
        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, objectResult.StatusCode);

        var response = Assert.IsType<Response<string>>(objectResult.Value);
        Assert.Equal("Erro ao atualizar venda: Venda não encontrada", response.Errors.First());
    }

    [Fact]
    public void PutAsync_WhenInvalidOperation_ReturnsBadRequest()
    {
        // Arrange
        _salesServiceMock
            .Setup(s => s.UpdateSaleStatus(It.IsAny<int>(), ESaleStatus.Delivered))
            .Throws(new InvalidOperationException("Transição de status inválida"));

        // Act
        var result = _controller.PutAsync(1, new UpdateSaleStatusRequest { Status = ESaleStatus.Delivered });

        // Assert
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, objectResult.StatusCode);

        var response = Assert.IsType<Response<string>>(objectResult.Value);
        Assert.Equal("Erro ao atualizar venda: Transição de status inválida", response.Errors.First());
    }

    [Fact]
    public void PutAsync_WhenUnexpectedErrorOccurs_ReturnsInternalServerError()
    {
        // Arrange
        _salesServiceMock
            .Setup(s => s.UpdateSaleStatus(It.IsAny<int>(), ESaleStatus.Delivered))
            .Throws(new Exception("Falha interna no servidor"));

        // Act
        var result = _controller.PutAsync(1, new UpdateSaleStatusRequest { Status = ESaleStatus.Delivered });

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);

        var response = Assert.IsType<Response<string>>(objectResult.Value);
        Assert.Equal("Falha interna no servidor", response.Errors.First());
    }
}
