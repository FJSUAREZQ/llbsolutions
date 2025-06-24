using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class VentaDetalleDTO
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "El identificador de la venta es obligatorio.")]
        public int SaleId { get; set; }


        [Required(ErrorMessage = "El identificador del producto es obligatorio.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string? ProductName { get; set; }


        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public int Quantity { get; set; }


        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor que cero.")]
        public decimal UnitPrice { get; set; }


        [Range(0, double.MaxValue, ErrorMessage = "El descuento no puede ser negativo.")]
        public decimal? Discount { get; set; }
    }
}
