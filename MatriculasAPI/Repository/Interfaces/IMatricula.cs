using MatriculasMODELS;
namespace MatriculasAPI.Repository.Interfaces
{
    public interface IMatricula
    {
        IEnumerable<Matricula> aMatricula(int id_matricula);
    }
}
