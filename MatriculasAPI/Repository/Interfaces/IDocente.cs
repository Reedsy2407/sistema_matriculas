using MatriculasAPI.Models;

namespace MatriculasAPI.Repository.Interfaces
{
    public interface IDocente
    {

        IEnumerable<Docente> aDocentes();
        Docente buscarDocente(int id);
        bool registrarDocente(Docente objD);
        bool actualizarDocente(Docente objD);
    }
}
