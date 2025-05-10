using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatriculasMODELS
{
    public class HorarioCursoNuevo
    {
        [Required]
        public int id_curso { get; set; }

        public int? id_seccion { get; set; }

        public string? cod_seccion { get; set; }
        [Required]
        public int id_aula { get; set; }
        [Required]
        public int id_docente { get; set; }
        [Required]
        public int cupos_maximos { get; set; }
        [Required]
        public string? tipo_horario { get; set; }
        [Required]
        public string? hora_inicio { get; set; }
        [Required]
        public string? hora_fin { get; set; }
        [Required] 
        public int dia_semana { get; set; }

    }
}
