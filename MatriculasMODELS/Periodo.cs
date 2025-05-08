using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatriculasMODELS
{
    public class Periodo
    {
        public int id_periodo { get; set; }
        public string? codigo_periodo { get; set; }
        public DateTime fcha_inicio { get; set; }
        public DateTime fcha_fin { get; set; }
    }
}
