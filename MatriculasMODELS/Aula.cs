using System.ComponentModel;

namespace MatriculasMODELS
{
    public class Aula
    {
        [DisplayName("AULA")]
        public string? cod_aula { get; set; }

        [DisplayName("CAPACIDAD")]
        public int capacidad_aula { get; set; }

        [DisplayName("DISPONIBILIDAD")]
        public bool es_disponible { get; set; }
    }
}
