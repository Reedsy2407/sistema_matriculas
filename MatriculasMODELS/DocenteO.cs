using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MatriculasMODELS
{
    public class DocenteO
    {
        [DisplayName("ID")]
        [Required]
        public int id_docente { get; set; }

        [DisplayName("NOMBRE")]
        [Required]
        public string? nom_docente { get; set; }

        [DisplayName("APELLIDO")]
        [Required]
        public string? ape_docente { get; set; }

        [DisplayName("CORREO")]
        [Required]
        public string? correo { get; set; }

        [DisplayName("ESPECIALIDAD")]
        [Required]
        public int cod_especialidad { get; set; }
        public bool estado { get; set; }
    }
}
