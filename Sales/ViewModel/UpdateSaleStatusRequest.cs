using Sales.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sales.ViewModel;

public class UpdateSaleStatusRequest
{
    [Required(ErrorMessage = "Novo status da venda é obrigatório")]
    public ESaleStatus Status { get; set; }
}