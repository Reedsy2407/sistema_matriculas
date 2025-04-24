using MatriculasAPI.Models;

namespace MatriculasAPI.Repository.Interfaces
{
    public interface ICurso
    {
        IEnumerable<Curso> aCursos();
        Curso buscarCurso(int id);
        bool registrarCurso(Curso objC);
        bool actualizarCurso(Curso objC);
    }
}
