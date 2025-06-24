using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Productos
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public float Price { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Image { get; set; }
    }
}
