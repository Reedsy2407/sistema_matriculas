using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Matriculas.Models
{
    public class Especialidad
    {
        [DisplayName("CODIGO")]
        [Required]
        public int cod_especialidad { get; set; }
        [DisplayName("ESPECIALIDAD")]
        [Required]
        public string nom_especialidad { get; set; }
    }
}
