using System.ComponentModel;

namespace MatriculasMODELS
{
    public class Docente
    {
        [DisplayName("ID")]
        public int id_docente { get; set; }

        [DisplayName("NOMBRE")]
        public string? nom_docente { get; set; }

        [DisplayName("ESPECIALIDAD")]
        public string? nom_especialidad { get; set; }

        [DisplayName("ESTADO")]
        public bool estado { get; set; }
    }
}
