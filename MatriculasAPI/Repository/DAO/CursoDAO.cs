using MatriculasAPI.Repository.Interfaces;
using MatriculasMODELS;
using System.Data.SqlClient;

namespace MatriculasAPI.Repository.DAO
{
    public class CursoDAO : ICurso
    {
        private readonly string cadena = "";
        public CursoDAO() 
        { 
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }
        public bool actualizarCurso(Curso objC)
        {
            bool exito = false;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_actualizarCurso", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id_curso", objC.id_curso);
            cmd.Parameters.AddWithValue("@nom_curso", objC.nom_curso);
            cmd.Parameters.AddWithValue("@creditos_curso", objC.creditos_curso);
            cmd.Parameters.AddWithValue("@id_carrera", objC.id_carrera);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                exito = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar curso: " + ex.Message);
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return exito;
        }

        public IEnumerable<Curso> aCursos()
        {
            List<Curso> lista = new List<Curso>();
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_listarCursos", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Curso
                {
                    id_curso = int.Parse(dr[0].ToString()),
                    nom_curso = dr[1].ToString(),
                    creditos_curso = int.Parse(dr[2].ToString()),
                    id_carrera = int.Parse(dr[3].ToString()),
                    nom_carrera = dr[4].ToString()
                });
            }
            dr.Close();
            con.Close();
            cmd.Dispose();
            return lista;
        }

        public Curso buscarCurso(int id)
        {
            Curso curso = null;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_buscarCurso", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_curso", id);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                curso = new Curso
                {
                    id_curso = int.Parse(dr[0].ToString()),
                    nom_curso = dr[1].ToString(),
                    creditos_curso = int.Parse(dr[2].ToString()),
                    id_carrera = int.Parse(dr[3].ToString()),
                    nom_carrera = dr[4].ToString()
                };
            }
            dr.Close();
            con.Close();
            cmd.Dispose();

            return curso;
        }

        public bool registrarCurso(Curso objC)
        {           
            bool exito = false;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_registrarCurso", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nom_curso", objC.nom_curso);
            cmd.Parameters.AddWithValue("@creditos_curso", objC.creditos_curso);
            cmd.Parameters.AddWithValue("@id_carrera", objC.id_carrera);

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
