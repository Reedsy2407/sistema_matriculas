using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatriculasMODELS
{
    public class Matriculas
    {
        [DisplayName("ID")]
        public int IdMatricula { get; set; }
        public int IdUsuario { get; set; }
        [DisplayName("USUARIO")]
        public string? NombreCompleto { get; set; }
        [DisplayName("PERIODO")]
        public string? CodigoPeriodo { get; set; }
        public int IdCarrera { get; set; }
        [DisplayName("CARRERA")]
        public string? NomCarrera { get; set; }
        public int IdCurso { get; set; }
        [DisplayName("CURSO")]
        public string NomCurso { get; set; }
        [DisplayName("CREDITOS")]
        public int CreditosCurso { get; set; }
        public int IdSeccion { get; set; }
        [DisplayName("SECCION")]
        public string? CodSeccion { get; set; }
        public int IdAula { get; set; }
        [DisplayName("CODIGO AULA")]
        public string? CodAula { get; set; }
        [DisplayName("DIA")]
        public string nomDiaSemana { get; set; }
        [DisplayName("HORA INICIO")]
        public TimeSpan horaInicio { get; set; }
        [DisplayName("HORA FIN")]
        public TimeSpan horaFin { get; set; }
        [DisplayName("TIPO CLASE")]
        public string tipoHorario { get; set; }
    }
}
