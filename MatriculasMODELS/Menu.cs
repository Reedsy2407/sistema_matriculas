using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatriculasMODELS
{
    public class Menu
    {
        public int id_menu { get; set; }
        public string titulo_menu { get; set; }
        public string url_menu { get; set; }
        public int orden { get; set; }
        public bool es_activo { get; set; }
    }
}
