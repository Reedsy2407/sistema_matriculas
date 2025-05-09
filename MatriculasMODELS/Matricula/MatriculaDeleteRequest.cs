using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatriculasMODELS.Matricula
{
    public class MatriculaDeleteRequest
    {
        public int IdAlumno { get; set; }
        public int IdSeccion { get; set; }
        public int IdPeriodo { get; set; }
    }
}
