using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Clientes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Address_City { get; set; } // Ciudad del cliente
        public string Address_Street { get; set; } // Dirección del cliente
        public string Address_Suite { get; set; } // Suite del cliente
        public string Email { get; set; }
        public int LoyaltyPoints { get; set; } // Puntos de fidelización
        public DateTime? CreatedDate { get; set; }
    }
}
