using MatriculasMODELS;

namespace MatriculasAPI.Repository.Interfaces
{
    public interface ICarrera
    {

        IEnumerable<Carrera> listarCarreras();
        IEnumerable<Carrera> listarCarrerasPorUsuario(int id_usuario);
        bool eliminarCarrerasUsuario(int id_usuario);
        bool asignarCarreraUsuario(int id_usuario, int id_carrera);
    }
}
