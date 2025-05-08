using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatriculasMODELS.Matricula
{
    public class MatriculaRequest
    {
        public int IdAlumno { get; set; }
        public int IdCarrera { get; set; }
        public int IdCurso { get; set; }
        public int IdSeccion { get; set; }
        public int IdPeriodo { get; set; }
    }
}
