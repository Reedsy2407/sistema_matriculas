using MatriculasAPI.Repository.Interfaces;
using MatriculasMODELS.Login;
using System.Data.SqlClient;

namespace MatriculasAPI.Repository.DAO
{
    public class LoginDAO : ILogin
    {
        private readonly string cadena;

        public LoginDAO()
        {
            cadena = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("cn");
        }

        public LoginResponse login(LoginRequest request)
        {
            var response = new LoginResponse
            {
                es_exito = false,
                mensaje = "Error desconocido",
                id_usuario = 0,
                id_rol = 0
            };

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_LoginUsuario", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // Parámetros de entrada
                cmd.Parameters.AddWithValue("@correo", request.correo);
                cmd.Parameters.AddWithValue("@contrasena", request.contrasena);

                // Parámetros de salida
                SqlParameter outLoginExitoso = new SqlParameter("@login_exitoso", System.Data.SqlDbType.Bit);
                outLoginExitoso.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(outLoginExitoso);

                SqlParameter outMensaje = new SqlParameter("@mensaje", System.Data.SqlDbType.VarChar, 100);
                outMensaje.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(outMensaje);

                SqlParameter outIdUsuario = new SqlParameter("@id_usuario", System.Data.SqlDbType.Int);
                outIdUsuario.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(outIdUsuario);

                SqlParameter outIdRol = new SqlParameter("@id_rol", System.Data.SqlDbType.Int);
                outIdRol.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(outIdRol);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();

                    // Obtener valores de los parámetros de salida
                    response.es_exito = (bool)outLoginExitoso.Value;
                    response.mensaje = outMensaje.Value.ToString();
                    response.id_usuario = (int)outIdUsuario.Value;
                    response.id_rol = (int)outIdRol.Value;
                }
                catch (Exception ex)
                {
                    response.mensaje = $"Error al autenticar usuario: {ex.Message}";
                }
                finally
                {
                    con.Close();
                    cmd.Dispose();
                }
            }

            return response;
        }
    }
}
