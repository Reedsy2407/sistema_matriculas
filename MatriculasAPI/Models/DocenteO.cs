using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MatriculasAPI.Models
{
    public class DocenteO
    {
        public int id_docente { get; set; }
        public string nom_docente { get; set; }
        public int cod_especialidad { get; set; }
        public bool estado { get; set; }
    }
}
