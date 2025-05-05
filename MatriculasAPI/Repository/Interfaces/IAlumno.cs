using MatriculasMODELS;

namespace MatriculasAPI.Repository.Interfaces
{
    public interface IAlumno
    {
        IEnumerable<Alumno> aAlumnos();
        AlumnoO buscarAlumno(int id);
        bool registrarAlumno(AlumnoO objA);
        bool actualizarAlumno(AlumnoO objA);
    }
}
