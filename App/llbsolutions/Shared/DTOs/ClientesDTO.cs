using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ClientesDTO
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede superar los 50 caracteres.")]
        public string Username { get; set; }


        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [StringLength(100, ErrorMessage = "La ciudad no puede superar los 100 caracteres.")]
        public string City { get; set; }


        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres.")]
        public string Street { get; set; }


        [Required(ErrorMessage = "La suite es obligatoria.")]
        [StringLength(100, ErrorMessage = "La suite no puede superar los 100 caracteres.")]
        public string Suite { get; set; } 


        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Email { get; set; }


        [Range(0, int.MaxValue, ErrorMessage = "Los puntos de fidelidad no pueden ser negativos.")]
        public int LoyaltyPoints { get; set; } // Puntos de fidelización


    }
}
