using System.ComponentModel.DataAnnotations;

namespace appHospital.Models
{
    public class Rol
    {
        [Key]
        public int Id_Rol { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Ingrese el nombre del rol")]
        [Display(Name = "Nombre del rol")]
        public string? NombreRol { get; set; }

    }
}
