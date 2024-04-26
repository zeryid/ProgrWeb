using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appHospital.Models
{
   public class UsuarioRol 
    {

        [ForeignKey("Usuario")]
        public int Id_Usuario { get; set; }

        public Usuario? Usuario { get; set; }


        [ForeignKey("Rol")]
        public int Id_Rol { get; set; }

        public Rol? Rol { get; set; }
    }
}
