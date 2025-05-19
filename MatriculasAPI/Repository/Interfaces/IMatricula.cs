using MatriculasMODELS;
namespace MatriculasAPI.Repository.Interfaces
{
    public interface IMatricula
    {
        IEnumerable<Matriculas> aMatricula(int id_usuario);
    }
}
