using Sales.Models;
using System.ComponentModel.DataAnnotations;

namespace Sales.ViewModel;

public class SaleRequest
{
    [Required(ErrorMessage = "O vendedor é obrigatório")]
    public Seller Seller { get; set; }

    [Required(ErrorMessage = "Ao menos um item de venda é obrigatório")]
    public List<Item> Items { get; set; }
}