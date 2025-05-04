using MatriculasMODELS.Login;

namespace MatriculasAPI.Repository.Interfaces
{
    public interface ILogin
    {
        LoginResponse login(LoginRequest request);
    }
}
