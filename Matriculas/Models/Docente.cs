using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Matriculas.Models
{
    public class Docente
    {

        [DisplayName("DOCENTE")]
        [Required]
        public int id_docente { get; set; }

        [DisplayName("NOMBRES")]
        [Required]
        public string nom_docente { get; set; }

        [DisplayName("ESPECIALIDAD")]
        [Required]
        public string nom_especialidad { get; set; }

        [DisplayName("ESTADO")]
        [Required]
        public bool activo { get; set; }
    }
}
