using MatriculasMODELS;

namespace MatriculasAPI.Repository.Interfaces
{
    public interface IAula
    {

        IEnumerable<Aula> aAulas();

        Aula buscarAula(int cod);
        bool registrarAula(Aula objA);
        bool actualizarAula(Aula objA);
    }
}
