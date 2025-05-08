using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatriculasMODELS
{
    public class Matricula
    {
        [DisplayName("ID")]
        public int IdMatricula { get; set; }
        public int IdUsuario { get; set; }
        [DisplayName("USUARIO")]
        public string NombreCompleto { get; set; }
        [DisplayName("PERIODO")]
        public string CodigoPeriodo { get; set; }
        public int IdCarrera { get; set; }
        [DisplayName("CARRERA")]
        public string NomCarrera { get; set; }
        public int IdCurso { get; set; }
        [DisplayName("CURSO")]
        public string NomCurso { get; set; }
        [DisplayName("CREDITOS")]
        public int CreditosCurso { get; set; }
        public int IdSeccion { get; set; }
        [DisplayName("SECCION")]
        public string CodSeccion { get; set; }
        public int IdAula { get; set; }
        [DisplayName("CODIGO AULA")]
        public string CodAula { get; set; }
    }
}
