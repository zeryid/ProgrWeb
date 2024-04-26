using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace appHospital.Models
{
    public class Usuario: IdentityUser
    {
        //[Key]
        //public int Id_Usuario { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Ingrese el nombre del rol")]
        [Display(Name = "Nombre del rol")]
        public string? Usuarios { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(50)]
        [Required(ErrorMessage = "Ingrese el nombre del rol")]
        [Display(Name = "Nombre del rol")]
        public string? Contraseña { get; set; }

        [MaxLength(10)]
        [Required(ErrorMessage = "Ingrese el nombre del rol")]
        [Display(Name = "Nombre del rol")]
        public string? Estado { get; set; }

    }
}
