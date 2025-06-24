using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ApiResponse
{
    public class API_CustomersDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public API_AddressDto Address { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public API_CompanyDto Company { get; set; }

    }
}
