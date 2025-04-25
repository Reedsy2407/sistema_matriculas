using MatriculasAPI.Models;
using MatriculasAPI.Repository.Interfaces;
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
            throw new NotImplementedException();
        }

        public IEnumerable<Curso> aCursos()
        {
            List<Curso> lista = new List<Curso>();
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("USP_LISTAR_CURSOS", con);
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
                });
            }
            dr.Close();
            con.Close();
            cmd.Dispose();
            return lista;
        }

        public Curso buscarCurso(int id)
        {
            throw new NotImplementedException();
        }

        public bool registrarCurso(Curso objC)
        {           
            bool exito = false;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("USP_REGISTRAR_CURSO", con);
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
