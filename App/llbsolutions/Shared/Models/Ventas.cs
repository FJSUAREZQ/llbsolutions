using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Ventas
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public decimal Subtotal { get; set; }
        public int PointsEarned { get; set; }
        public int PointsUsed { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<VentaDetalle> Details { get; set; } = new();
    }

    public enum PaymentMethod
    {
        Cash,
        CreditCard,
        DebitCard,
        BankTransfer,
        Other
    }
}
