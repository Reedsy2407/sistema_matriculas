using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatriculasMODELS
{
    public class HorarioPorCurso
    {
        public int id_seccion { get; set; }
        public string cod_seccion { get; set; }
        public string dia_semana { get; set; }
        public string hora_inicio { get; set; }
        public string hora_fin { get; set; }
        public string tipo_horario { get; set; }
        public string cod_aula { get; set; }
        public string nombre_docente { get; set; }
        public int cupos_disponible { get; set; }
        public int cupos_maximos { get; set; }
        public string nom_curso { get; set; }
    }
}
