using MatriculasAPI.Repository.Interfaces;
using MatriculasMODELS;
using System.Data.SqlClient;
using System.Data;

namespace MatriculasAPI.Repository.DAO
{
    public class ClaseDAO : IClase
    {
        private readonly string cadena;

        public ClaseDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }

        public Periodo BuscarPeriodoActual()
        {
            Periodo periodo = null;
            using (SqlConnection con = new SqlConnection(cadena))
            {
                using (SqlCommand cmd = new SqlCommand("usp_BuscarPeriodoActual", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            periodo = new Periodo
                            {
                                id_periodo = Convert.ToInt32(dr["id_periodo"]),
                                codigo_periodo = dr["codigo_periodo"].ToString(),
                                fcha_inicio = Convert.ToDateTime(dr["fcha_inicio"]),
                                fcha_fin = Convert.ToDateTime(dr["fcha_fin"])
                            };
                        }
                    }
                }
            }
            return periodo;
        }

        public IEnumerable<Carrera> listarCarrerasPorUsuario(int idUsuario)
        {
            List<Carrera> carreras = new List<Carrera>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                using (SqlCommand cmd = new SqlCommand("uspListarCarrerasPorUsuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_usuario", idUsuario);

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            carreras.Add(new Carrera
                            {
                                id_carrera = Convert.ToInt32(dr["id_carrera"]),
                                nom_carrera = dr["nom_carrera"].ToString()
                            });
                        }
                    }
                }
            }

            return carreras;
        }

        public IEnumerable<Curso> listarCursosPorCarrera(int idCarrera)
        {
            var cursos = new List<Curso>();

            using (var con = new SqlConnection(cadena))
            {
                using (var cmd = new SqlCommand("uspListarCursosPorCarrera", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_carrera", idCarrera);

                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cursos.Add(new Curso
                            {
                                id_curso = Convert.ToInt32(dr["id_curso"]),
                                nom_curso = dr["nom_curso"].ToString(),
                                creditos_curso = Convert.ToInt32(dr["creditos_curso"]),
                                nom_carrera = dr["nom_carrera"].ToString(),
                                fcha_inicio = Convert.ToDateTime(dr["fecha_inicio_periodo"]),
                                fcha_fin = Convert.ToDateTime(dr["fecha_fin_periodo"])
                            });
                        }
                    }
                }
            }

            return cursos;
        }

        public Carrera buscarCarreraPorId(int idCarrera)
        {
            using (var con = new SqlConnection(cadena))
            {
                using (var cmd = new SqlCommand("uspBuscarCarreraPorId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_carrera", idCarrera);

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return new Carrera
                            {
                                id_carrera = Convert.ToInt32(dr["id_carrera"]),
                                nom_carrera = dr["nom_carrera"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
