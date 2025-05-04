using MatriculasMODELS;

namespace MatriculasAPI.Repository.Interfaces
{
    public interface IDocente
    {

        IEnumerable<Docente> aDocentes();
        DocenteO buscarDocente(int id);
        bool registrarDocente(DocenteO objD);
        bool actualizarDocente(DocenteO objD);
    }
}
