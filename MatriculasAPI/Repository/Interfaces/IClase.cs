using MatriculasMODELS;

namespace MatriculasAPI.Repository.Interfaces
{
    public interface IClase
    {
        Periodo BuscarPeriodoActual();
        IEnumerable<Carrera> listarCarrerasPorUsuario(int idUsuario);
        IEnumerable<Curso> listarCursosPorCarrera(int idCarrera);
        Carrera buscarCarreraPorId(int idCarrera);
        IEnumerable<HorarioPorCurso> ListarHorariosPorCurso(int id_curso);

    }
}
