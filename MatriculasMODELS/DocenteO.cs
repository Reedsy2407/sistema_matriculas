using System.ComponentModel;

namespace MatriculasMODELS
{
    public class DocenteO
    {
        [DisplayName("ID")]
        public int id_docente { get; set; }

        [DisplayName("Nombre")]
        public string? nom_docente { get; set; }

        [DisplayName("Apellido")]
        public string ape_docente { get; set; }

        [DisplayName("Correo")]
        public string correo { get; set; }

        [DisplayName("Especialidad")]
        public int cod_especialidad { get; set; }
        public bool estado { get; set; }
    }
}
