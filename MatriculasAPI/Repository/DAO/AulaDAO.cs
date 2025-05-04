using MatriculasAPI.Repository.Interfaces;
using MatriculasMODELS;
using System.Data;
using System.Data.SqlClient;

namespace MatriculasAPI.Repository.DAO
{
    public class AulaDAO : IAula
    {
        private readonly string cadena;

        public AulaDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }

        public IEnumerable<Aula> aAulas()
        {
            List<Aula> lista = new List<Aula>();
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_listarAulas", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Aula
                {
                    cod_aula = dr[0].ToString(),
                    capacidad_aula = int.Parse(dr[1].ToString()),
                    es_disponible = bool.Parse(dr[2].ToString())
                });
            }
            dr.Close();
            con.Close();
            cmd.Dispose();
            return lista;
        }

        public Aula buscarAula(string cod)
        {
            Aula aula = null;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_buscarAula", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cod_aula", cod);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                aula = new Aula
                {
                    cod_aula = dr[0].ToString(),
                    capacidad_aula = int.Parse(dr[1].ToString()),
                    es_disponible = bool.Parse(dr[2].ToString())
                };
            }
            dr.Close();
            con.Close();
            cmd.Dispose();
            return aula;
        }

        public bool registrarAula(Aula objA)
        {
            bool exito = false;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_registrarAulas", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@cod_aula", objA.cod_aula);
            cmd.Parameters.AddWithValue("@capacidad_aula", objA.capacidad_aula);
            cmd.Parameters.AddWithValue("@es_disponible", objA.es_disponible);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                exito = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar aula: " + ex.Message);
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return exito;
        }

        public bool actualizarAula(Aula objA)
        {
            bool exito = false;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_actualizarAulas", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@cod_aula", objA.cod_aula);
            cmd.Parameters.AddWithValue("@capacidad_aula", objA.capacidad_aula);
            cmd.Parameters.AddWithValue("@es_disponible", objA.es_disponible);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                exito = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar aula: " + ex.Message);
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
