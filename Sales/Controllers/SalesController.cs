using Microsoft.AspNetCore.Mvc;
using Sales.Extensions;
using Sales.Models;
using Sales.Services.Interfaces;
using Sales.ViewModel;

namespace Sales.Controllers;

[ApiController]
public class SalesController : ControllerBase
{
    private readonly ISalesService _salesService;

    public SalesController(ISalesService salesService)
    {
        _salesService = salesService;
    }

    [HttpPost("v1/sales")]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status500InternalServerError)]
    public IActionResult PostAsync([FromBody] SaleRequest saleRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(new Response<string>(ModelState.GetErrors()));

        try
        {
            var sale = _salesService.RegisterSale(saleRequest);
            return Ok(new Response<string>($"Venda registrada com sucesso! Id: {sale.Id}", null));
        }

        catch
        {
            return StatusCode(500, new Response<string>("Falha interna no servidor"));
        }
    }

    [HttpGet("v1/sales/{id:int}")]
    [ProducesResponseType(typeof(Response<Sale>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status500InternalServerError)]
    public IActionResult GetAsync([FromRoute] int id)
    {
        try
        {
            var sale = _salesService.GetSale(id);
            if (sale == null)
                return NotFound(new Response<Sale>("Venda não encontrada"));
            
            return Ok(new Response<Sale>(sale));
        }

        catch
        {
            return StatusCode(500, new Response<string>("Falha interna no servidor"));
        }
    }

    [HttpPut("v1/sales/{id:int}")]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<string>), StatusCodes.Status500InternalServerError)]
    public IActionResult PutAsync([FromRoute] int id, [FromBody] UpdateSaleStatusRequest saleRequest)
    {
        try
        {
            _salesService.UpdateSaleStatus(id, saleRequest.Status);
            return Ok(new Response<string>("Venda atualizada com sucesso!", null));
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(new Response<string>($"Erro ao atualizar venda: {ex.Message}"));
        }

        catch (InvalidOperationException ex)
        {
            return BadRequest(new Response<string>($"Erro ao atualizar venda: {ex.Message}"));
        }

        catch
        {
            return StatusCode(500, new Response<string>("Falha interna no servidor"));
        }
    }

}
