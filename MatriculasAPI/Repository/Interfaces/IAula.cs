using MatriculasAPI.Models;

namespace MatriculasAPI.Repository.Interfaces
{
    public interface IAula
    {

        IEnumerable<Aula> aAulas();

        Aula buscarAula(string cod);
        bool registrarAula(Aula objA);
        bool actualizarAula(Aula objA);
    }
}
