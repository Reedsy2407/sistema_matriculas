using MatriculasAPI.Repository.Interfaces;
using MatriculasMODELS;
using System.Data;
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

        public bool AsignarHorarioConProcedure(HorarioCursoNuevo objH, out string mensaje)
        {
            mensaje = "";
            using var cn = new SqlConnection(cadena);
            cn.Open();
            using var cmd = new SqlCommand("uspAsignarHorarioCurso", cn)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Parámetros de entrada: id_seccion **y** cod_seccion siempre presentes
            cmd.Parameters.AddWithValue("@id_curso", objH.id_curso);

            // Si no hay sección existente, pasamos DBNull
            cmd.Parameters.Add("@id_seccion", SqlDbType.Int)    
               .Value = objH.id_seccion.HasValue ? (object)objH.id_seccion.Value : DBNull.Value;

            // Si no hay código de sección, pasamos DBNull
            cmd.Parameters.Add("@cod_seccion", SqlDbType.Char, 4)
               .Value = !string.IsNullOrWhiteSpace(objH.cod_seccion)
                        ? (object)objH.cod_seccion
                        : DBNull.Value;

            cmd.Parameters.AddWithValue("@id_aula", objH.id_aula);
            cmd.Parameters.AddWithValue("@id_docente", objH.id_docente);
            cmd.Parameters.AddWithValue("@cupos_maximos", objH.cupos_maximos);
            cmd.Parameters.AddWithValue("@tipo_horario", objH.tipo_horario!);
            cmd.Parameters.AddWithValue("@hora_inicio", TimeSpan.Parse(objH.hora_inicio!));
            cmd.Parameters.AddWithValue("@hora_fin", TimeSpan.Parse(objH.hora_fin!));
            cmd.Parameters.AddWithValue("@dia_semana", objH.dia_semana);

            // Parámetros de salida
            var pRes = cmd.Parameters.Add("@resultado", SqlDbType.Bit);
            pRes.Direction = ParameterDirection.Output;
            var pMsg = cmd.Parameters.Add("@mensaje", SqlDbType.VarChar, 200);
            pMsg.Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();

            bool ok = (bool)pRes.Value!;
            mensaje = pMsg.Value!.ToString()!;
            return ok;
        }

        public IEnumerable<Curso> ListarCursosSinHorario()
        {
            var lista = new List<Curso>();
            using var cn = new SqlConnection(cadena);
            using var cmd = new SqlCommand("usp_listarCursosSinHorario", cn) { CommandType = CommandType.StoredProcedure };
            cn.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Curso
                {
                    id_curso = dr.GetInt32(0),
                    nom_curso = dr.GetString(1),
                    creditos_curso = dr.GetInt16(2),
                    id_carrera = dr.GetInt32(3),
                    nom_carrera = dr.GetString(4)
                });
            }
            return lista;
        }

        public IEnumerable<Seccion> ListarSeccionesPorCurso(int id_curso)
        {
            var lista = new List<Seccion>();
            using var cn = new SqlConnection(cadena);
            using var cmd = new SqlCommand("usp_ListarSeccionesPorCurso", cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@id_curso", id_curso);

            cn.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Seccion
                {
                    id_seccion = dr.GetInt32(dr.GetOrdinal("id_seccion")),
                    cod_seccion = dr.GetString(dr.GetOrdinal("cod_seccion"))
                });
            }
            return lista;
        }
    }
}
