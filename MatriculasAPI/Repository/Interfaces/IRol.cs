using MatriculasMODELS;

namespace MatriculasAPI.Repository.Interfaces
{
    public interface IRol
    {
        IEnumerable<Rol> ListarRoles();
        Rol ObtenerRol(int id);
        bool RegistrarRol(Rol rol);
        bool ActualizarRol(Rol rol);
        bool EliminarRol(int id);
    }
}
