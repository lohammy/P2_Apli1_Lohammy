using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace P2_Apli1_Lohammy.Models
{
    public class RegistroPedidos
    {
        [Key]
        public int PedidoId { get; set; }

        [Required(ErrorMessage = "Esto es obligatorio")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Esto es obligatorio")]
        [RegularExpression("^[a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]+$", ErrorMessage = "En este campo solo se permiten letras.")]
        public string NombreCliente { get; set; }

        public decimal Total { get; set; }

        [InverseProperty("RegistroPedido")]
        public virtual ICollection<RegistroPedidosDetalle> PedidosDetalles { get; set; } = [];
    }
}
