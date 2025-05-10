using MatriculasMODELS;


namespace MatriculasAPI.Repository.Interfaces
{
    public interface ICurso
    {
        IEnumerable<Curso> aCursos();
        IEnumerable<Curso> ListarCursosSinHorario();
        IEnumerable<Seccion> ListarSeccionesPorCurso(int id_curso);
        Curso buscarCurso(int id);
        bool registrarCurso(Curso objC);
        bool actualizarCurso(Curso objC);
        bool AsignarHorarioConProcedure(HorarioCursoNuevo objH, out string mensaje);

    }
}
