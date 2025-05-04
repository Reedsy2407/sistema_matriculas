using MatriculasMODELS;

namespace MatriculasAPI.Repository.Interfaces
{
    public interface IMenu
    {
        IEnumerable<Menu> listarMenus();
        IEnumerable<Menu> listarMenusPorRol(int idRol);
        bool registrarMenu(Menu menu);
        bool asignarMenuARol(MenuRol menuRol);
    }
}
