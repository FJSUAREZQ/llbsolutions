using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class VentaDTO
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "El identificador del cliente es obligatorio.")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        public string? ClientName { get; set; }

        
        [Required(ErrorMessage = "La dirección del cliente es obligatoria.")]
        public string? ClientAddress { get; set; }


        [Required(ErrorMessage = "La fecha de la venta es obligatoria.")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }


        [Required(ErrorMessage = "El total de la venta es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a cero.")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "El Subtotal de la venta es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El Subtotal debe ser mayor a cero.")]
        public decimal Subtotal { get; set; }

        
        public int PointsEarned { get; set; }
        public int PointsUsed { get; set; }


        [Required(ErrorMessage = "Debe especificarse el método de pago.")]
        public PaymentMethod PaymentMethod { get; set; }


        [MinLength(1, ErrorMessage = "Debe haber al menos un detalle de venta.")]
        public List<VentaDetalleDTO> Details { get; set; } = new();
    }


    public enum PaymentMethod
    {
        [EnumMember(Value = "Efectivo")]
        Cash,

        [EnumMember(Value = "Tarjeta de Crédito")]
        CreditCard,

        [EnumMember(Value = "Tarjeta de Débito")]
        DebitCard,

        [EnumMember(Value = "Transferencia Bancaria")]
        BankTransfer

    }


}
