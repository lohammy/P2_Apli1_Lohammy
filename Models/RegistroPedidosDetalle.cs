using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P2_Apli1_Lohammy.Models
{
    public class RegistroPedidosDetalle
    {
        [Key]
        public int Id { get; set; }

        public int PedidoId { get; set; }
        public int ComponenteId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe introducir una cantidad valida")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe introducir un monto valido")]
        public decimal Precio { get; set; }

        [ForeignKey("PedidoId")]
        [InverseProperty("PedidosDetalles")]
        public virtual RegistroPedidos RegistroPedido { get; set; } = null;

        [ForeignKey("ComponenteId")]
        [InverseProperty("PedidosDetalles")]
        public virtual Componente Componentes { get; set; }
    }
}