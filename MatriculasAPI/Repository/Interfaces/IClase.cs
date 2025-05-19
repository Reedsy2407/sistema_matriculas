using MatriculasMODELS;
using MatriculasMODELS.Matricula;

namespace MatriculasAPI.Repository.Interfaces
{
    public interface IClase
    {
        Periodo BuscarPeriodoActual();
        IEnumerable<Carrera> listarCarrerasPorUsuario(int idUsuario);
        IEnumerable<Curso> listarCursosPorCarrera(int idCarrera);
        Carrera buscarCarreraPorId(int idCarrera);
        IEnumerable<HorarioPorCurso> ListarHorariosPorCurso(int id_curso, int id_periodo);

        MatriculaResponse InsertarMatriculaAlumno(MatriculaRequest request);
        MatriculaResponse EliminarMatriculaAlumno(MatriculaDeleteRequest request);
        bool VerificarMatriculaAlumno(int idAlumno, int idSeccion, int idPeriodo);



    }
}
