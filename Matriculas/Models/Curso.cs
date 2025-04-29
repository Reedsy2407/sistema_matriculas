using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Matriculas.Models
{
    public class Curso
    {
        [DisplayName("CURSO")]
        [Required]
        public int id_curso { get; set; }
        [DisplayName("NOMBRE")]
        [Required]
        public string nom_curso { get; set; }
        [DisplayName("CREDITOS")]
        [Required]
        public int creditos_curso { get; set; }
        [DisplayName("CARRERA")]
        [Required]
        public string nom_carrera { get; set; }
    }
}
