using System.ComponentModel.DataAnnotations;

namespace P2_Apli1_Lohammy.Models;

public class RegistroPedidos
{
    [Key] 
    public int PedidoId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "En este campo solo se permiten letras. ")]
    public string NombreCliente { get; set; }
    public decimal Total { get; set; }
}
