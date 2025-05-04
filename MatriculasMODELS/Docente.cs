using System.ComponentModel;

namespace MatriculasMODELS
{
    public class Docente
    {
        [DisplayName("ID")]
        public int id_docente { get; set; }

        [DisplayName("Nombre")]
        public string? nom_docente { get; set; }

        [DisplayName("Especialidad")]
        public string? nom_especialidad { get; set; }

        [DisplayName("Estado")]
        public bool estado { get; set; }
    }
}
