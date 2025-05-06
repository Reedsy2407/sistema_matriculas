using MatriculasAPI.Repository.Interfaces;
using MatriculasMODELS;
using System.Data.SqlClient;
using System.Data;

namespace MatriculasAPI.Repository.DAO
{
    public class AlumnoDAO : IAlumno
    {


        private readonly string cadena;

        public AlumnoDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }

        public IEnumerable<Alumno> aAlumnos()
        {
            List<Alumno> lista = new List<Alumno>();
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_listarAlumnos", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Alumno
                {
                    id_usuario = int.Parse(dr[0].ToString()),
                    nom_usuario = dr[1].ToString(),
                    correo = dr[2].ToString(),
                    contrasena = dr[3].ToString(),
                    estado = bool.Parse(dr[4].ToString())
                });
            }
            dr.Close();
            con.Close();
            cmd.Dispose();

            return lista;
        }

        public AlumnoO buscarAlumno(int id)
        {
            AlumnoO alumno = null;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_buscarAlumno", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_usuario", id);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                alumno = new AlumnoO
                {
                    id_usuario = int.Parse(dr[0].ToString()),
                    nom_usuario = dr[1].ToString(),
                    ape_usuario = dr[2].ToString(),
                    correo = dr[3].ToString(),
                    contrasena = dr[4].ToString(),
                    estado = bool.Parse(dr[5].ToString())
                };
            }
            dr.Close();
            con.Close();
            cmd.Dispose();

            return alumno;
        }

        public bool registrarAlumno(AlumnoO objA)
        {
            bool exito = false;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_registrarAlumno", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nom_usuario", objA.nom_usuario);
            cmd.Parameters.AddWithValue("@ape_usuario", objA.ape_usuario);
            cmd.Parameters.AddWithValue("@correo", objA.correo);
            cmd.Parameters.AddWithValue("@contrasena", objA.contrasena);
            cmd.Parameters.AddWithValue("@estado", objA.estado);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                exito = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar alumno: " + ex.Message);
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
            return exito;
        }

        public bool actualizarAlumno(AlumnoO objA)
        {
            bool exito = false;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_actualizarAlumno", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id_usuario", objA.id_usuario);
            cmd.Parameters.AddWithValue("@nom_usuario", objA.nom_usuario);
            cmd.Parameters.AddWithValue("@ape_usuario", objA.ape_usuario);
            cmd.Parameters.AddWithValue("@correo", objA.correo);
            cmd.Parameters.AddWithValue("@contrasena", objA.contrasena);
            cmd.Parameters.AddWithValue("@estado", objA.estado);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                exito = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar alumno: " + ex.Message);
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
