
using MatriculasAPI.Repository.Interfaces;
using MatriculasMODELS;
using System.Data.SqlClient;

namespace MatriculasAPI.Repository.DAO
{
    public class DocenteDAO : IDocente
    {

        private readonly string cadena = "";
        public DocenteDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }

        public bool actualizarDocente(DocenteO objD)
        {
            bool exito = false;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_actualizarDocentes", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id_docente", objD.id_docente);
            cmd.Parameters.AddWithValue("@nom_docente", objD.nom_docente);
            cmd.Parameters.AddWithValue("@ape_docente", objD.ape_docente);
            cmd.Parameters.AddWithValue("@correo", objD.correo);
            cmd.Parameters.AddWithValue("@cod_especialidad", objD.cod_especialidad);
            cmd.Parameters.AddWithValue("@estado", objD.estado);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                exito = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar docente: " + ex.Message);
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }

            return exito;
        }

        public IEnumerable<Docente> aDocentes()
        {
            List<Docente> lista = new List<Docente>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_listarDocentes", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Docente
                        {
                            id_docente = Convert.ToInt32(dr["id_docente"]),
                            nom_docente = dr["nom_docente"].ToString(),
                            nom_especialidad = dr["nom_especialidad"].ToString(),
                            estado = Convert.ToInt32(dr["estado"]) == 1
                        });
                    }
                }
            }
            return lista;
        }

        public IEnumerable<DocenteO> aDocentesO()
        {
            List<DocenteO> lista = new List<DocenteO>();
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_listarDocentes", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new DocenteO
                {
                    id_docente = int.Parse(dr[0].ToString()),
                    nom_docente = dr[1].ToString(),
                    cod_especialidad = int.Parse(dr[2].ToString()),
                    estado = bool.Parse(dr[3].ToString()),
                });
            }
            dr.Close();
            con.Close();
            cmd.Dispose();
            return lista;
        }


        public DocenteO buscarDocente(int id)
        {
            DocenteO docente = null;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_buscarDocente", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_docente", id);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                docente = new DocenteO
                {
                    id_docente = int.Parse(dr[0].ToString()),
                    nom_docente = dr[1].ToString(),
                    ape_docente = dr[2].ToString(),
                    correo = dr[3].ToString(),
                    cod_especialidad = int.Parse(dr[4].ToString()),
                    estado = bool.Parse(dr[6].ToString())
                };
            }
            dr.Close();
            con.Close();
            cmd.Dispose();

            return docente;
        }

        public bool registrarDocente(DocenteO objD)
        {
            bool exito = false;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_registrarDocentes", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nom_docente", objD.nom_docente);
            cmd.Parameters.AddWithValue("@ape_docente", objD.ape_docente);
            cmd.Parameters.AddWithValue("@correo", objD.correo);
            cmd.Parameters.AddWithValue("@cod_especialidad", objD.cod_especialidad);
            cmd.Parameters.AddWithValue("@estado", objD.estado);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                exito = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar curso: " + ex.Message);
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
