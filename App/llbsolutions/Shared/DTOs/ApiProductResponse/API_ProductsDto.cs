using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.ApiProductResponse
{
    public class API_ProductsDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public float Price { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Image { get; set; }

    }
}
