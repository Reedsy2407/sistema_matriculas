using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatriculasMODELS
{
    public class Alumno
    {

        [DisplayName("ID")]
        [Required]
        public int id_usuario { get; set; }

        [DisplayName("NOMBRES")]
        [Required]

        public string? nom_usuario { get; set; }

        [DisplayName("CORREO")]
        [Required]

        public string? correo { get; set; }

        [DisplayName("CONTRASEÑA")]
        [Required]

        public string? contrasena { get; set; }

        [DisplayName("ESTADO")]
        [Required]

        public bool estado { get; set; }


        [DisplayName("CARRERA")]
        [Required]
        public string? carreras { get; set; }
    }
}
