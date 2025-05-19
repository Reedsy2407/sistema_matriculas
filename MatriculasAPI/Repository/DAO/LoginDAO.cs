using MatriculasAPI.Repository.Interfaces;
using MatriculasMODELS.Login;
using System.Data.SqlClient;
using System.Data;

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
            LoginResponse response = new LoginResponse();
            response.es_exito = false;
            response.mensaje = "Error desconocido";
            response.id_usuario = 0;
            response.id_rol = 0;
            response.ultimo_acceso = null;

            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("sp_LoginUsuario", con);
            cmd.CommandType = CommandType.StoredProcedure;

            // Parámetros de entrada
            cmd.Parameters.AddWithValue("@correo", request.correo ?? "");
            cmd.Parameters.AddWithValue("@contrasena", request.contrasena ?? "");

            // Parámetros de salida
            SqlParameter pLoginExitoso = new SqlParameter("@login_exitoso", SqlDbType.Bit);
            pLoginExitoso.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pLoginExitoso);

            SqlParameter pMensaje = new SqlParameter("@mensaje", SqlDbType.VarChar, 100);
            pMensaje.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pMensaje);

            SqlParameter pIdUsuario = new SqlParameter("@id_usuario", SqlDbType.Int);
            pIdUsuario.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pIdUsuario);

            SqlParameter pIdRol = new SqlParameter("@id_rol", SqlDbType.Int);
            pIdRol.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pIdRol);

            SqlParameter pUltimoAcceso = new SqlParameter("@ultimo_acceso", SqlDbType.DateTime);
            pUltimoAcceso.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pUltimoAcceso);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();

                response.es_exito = Convert.ToBoolean(pLoginExitoso.Value);
                response.mensaje = pMensaje.Value.ToString();
                response.id_usuario = Convert.ToInt32(pIdUsuario.Value);
                response.id_rol = Convert.ToInt32(pIdRol.Value);

                if (pUltimoAcceso.Value != DBNull.Value)
                {
                    response.ultimo_acceso = Convert.ToDateTime(pUltimoAcceso.Value);
                }
            }
            catch (Exception ex)
            {
                response.mensaje = "Error al autenticar usuario: " + ex.Message;
            }
            finally
            {
                con.Close();
                cmd.Dispose();
                con.Dispose();
            }

            return response;
        }
    }
}