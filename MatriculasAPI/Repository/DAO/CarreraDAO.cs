using MatriculasAPI.Repository.Interfaces;
using MatriculasMODELS;
using System.Data.SqlClient;
using System.Data;

namespace MatriculasAPI.Repository.DAO
{
    public class CarreraDAO : ICarrera
    {
        private readonly string cadena;

        public CarreraDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }

        public IEnumerable<Carrera> listarCarreras()
        {
            List<Carrera> lista = new List<Carrera>();
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_listarCarreras", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Carrera
                {
                    id_carrera = int.Parse(dr[0].ToString()),
                    nom_carrera = dr[1].ToString()
                });
            }
            dr.Close();
            con.Close();
            cmd.Dispose();

            return lista;
        }

        public IEnumerable<Carrera> listarCarrerasPorUsuario(int id_usuario)
        {
            List<Carrera> lista = new List<Carrera>();
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("uspListarCarrerasPorUsuario", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_usuario", id_usuario);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Carrera
                {
                    id_carrera = int.Parse(dr[0].ToString()),
                    nom_carrera = dr[1].ToString()
                });
            }
            dr.Close();
            con.Close();
            cmd.Dispose();

            return lista;
        }


        public bool asignarCarreraUsuario(int id_usuario, int id_carrera)
        {
            bool exito = false;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_asignarCarreraUsuario", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_usuario", id_usuario);
            cmd.Parameters.AddWithValue("@id_carrera", id_carrera);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                exito = true;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return exito;
        }

        public bool eliminarCarrerasUsuario(int id_usuario)
        {
            bool exito = false;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_eliminarCarrerasUsuario", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_usuario", id_usuario);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                exito = true;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return exito;
        }


    }
}
