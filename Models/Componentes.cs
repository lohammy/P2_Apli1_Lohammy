using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace P2_Apli1_Lohammy.Models
{
    public class Componente
    {
        [PrimaryKey]
        public int ComponenteId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe introducir una cantidad valida")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe introducir una cantidad valida")]
        public int Existencia { get; set; }
    }
}
