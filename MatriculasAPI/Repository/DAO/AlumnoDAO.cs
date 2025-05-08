using MatriculasAPI.Repository.Interfaces;
using MatriculasMODELS;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace MatriculasAPI.Repository.DAO
{
    public class AlumnoDAO : IAlumno
    {


        private readonly string cadena;
        private readonly CarreraDAO carreraDAO;


        public AlumnoDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
            carreraDAO = new CarreraDAO();
        }

        public IEnumerable<Alumno> aAlumnos()
        {
            List<Alumno> lista = new List<Alumno>();
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_listarAlumnos", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int id = int.Parse(dr[0].ToString());
                var carreras = carreraDAO
                                    .listarCarrerasPorUsuario(id)
                                    .Select(c => c.nom_carrera);

                string texto = string.Join(", ", carreras);

                lista.Add(new Alumno
                {
                    id_usuario = id,
                    nom_usuario = dr[1].ToString(),
                    correo = dr[2].ToString(),
                    contrasena = dr[3].ToString(),
                    estado = bool.Parse(dr[4].ToString()),
                    carreras = texto
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

            // 1.1) Cargar sus carreras
            IEnumerable<Carrera> listaCarr = carreraDAO.listarCarrerasPorUsuario(id);
            alumno.id_carreras.Clear();
            foreach (Carrera c in listaCarr)
            {
                alumno.id_carreras.Add(c.id_carrera);
            }

            return alumno;
        }


        public bool registrarAlumno(AlumnoO objA)
        {
            bool exito = false;
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_registrarAlumno", con);
            cmd.CommandType = CommandType.StoredProcedure;

            // Parámetros de inserción
            cmd.Parameters.AddWithValue("@nom_usuario", objA.nom_usuario);
            cmd.Parameters.AddWithValue("@ape_usuario", objA.ape_usuario);
            cmd.Parameters.AddWithValue("@correo", objA.correo);
            cmd.Parameters.AddWithValue("@contrasena", objA.contrasena);
            cmd.Parameters.AddWithValue("@estado", objA.estado);

            // Parámetro OUTPUT para capturar el nuevo ID
            SqlParameter pOut = new SqlParameter("@new_id_usuario", SqlDbType.Int);
            pOut.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(pOut);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                // Obtener el ID generado
                int nuevoId = int.Parse(pOut.Value.ToString());
                // Primero eliminar cualquier asignación previa (por si acaso)
                carreraDAO.eliminarCarrerasUsuario(nuevoId);
                // Ahora asignar las carreras seleccionadas
                foreach (int idCarr in objA.id_carreras)
                {
                    carreraDAO.asignarCarreraUsuario(nuevoId, idCarr);
                }

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

            // Parámetros de actualización
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

                // 3.1) Borrar las carreras actuales
                carreraDAO.eliminarCarrerasUsuario(objA.id_usuario);

                // 3.2) Volver a asignar las nuevas
                foreach (int idCarr in objA.id_carreras)
                {
                    carreraDAO.asignarCarreraUsuario(objA.id_usuario, idCarr);
                }

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

        public string ObtenerHorariosyCursosMatriculados(int idUsuario, int?idPeriodo = null)
        {
            var sb = new StringBuilder();
            using (var cn = new SqlConnection(cadena))
            using (var cmd = new SqlCommand("usp_listarCursosYHorariosMatriculados", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_usuario", idUsuario);
                cmd.Parameters.AddWithValue("@id_periodo", idPeriodo.HasValue ? (object)idPeriodo.Value : DBNull.Value);

                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    if (!dr.HasRows)
                    {
                        return "<span class=\"text-muted\">Sin matrícula</span>";
                    }

                    sb.Append("<ul class=\"list-unstyled mb-0\">");
                    while (dr.Read())
                    {
                        var curso = dr.GetString(0);
                        var dia = dr.GetString(1);
                        var hi = dr.GetString(2);
                        var hf = dr.GetString(3);
                        sb.AppendFormat("<li><strong>{0}</strong> — {1} ({2}–{3})</li>", curso, dia, hi, hf);
                    }
                    sb.Append("</ul>");
                }
            }
            return sb.ToString();
        }
    }
}
