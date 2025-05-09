using MatriculasAPI.Repository.Interfaces;
using MatriculasMODELS;
using System.Data.SqlClient;
using System.Data;
using MatriculasMODELS.Matricula;

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

        public IEnumerable<HorarioPorCurso> ListarHorariosPorCurso(int id_curso)
        {
            var horarios = new List<HorarioPorCurso>();

            using (var con = new SqlConnection(cadena))
            {
                using (var cmd = new SqlCommand("uspListarHorariosPorCurso", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_curso", id_curso);

                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            horarios.Add(new HorarioPorCurso
                            {
                                id_seccion = Convert.ToInt32(dr["id_seccion"]),
                                cod_seccion = dr["cod_seccion"].ToString(),
                                dia_semana = dr["dia_semana"].ToString(),
                                hora_inicio = dr["hora_inicio"].ToString(),
                                hora_fin = dr["hora_fin"].ToString(),
                                tipo_horario = dr["tipo_horario"].ToString(),
                                cod_aula = dr["cod_aula"].ToString(),
                                nombre_docente = dr["nombre_docente"].ToString(),
                                cupos_disponible = Convert.ToInt32(dr["cupos_disponible"]),
                                cupos_maximos = Convert.ToInt32(dr["cupos_maximos"]),
                                nom_curso = dr["nom_curso"].ToString()
                            });
                        }
                    }
                }
            }

            return horarios;
        }

        public MatriculaResponse InsertarMatriculaAlumno(MatriculaRequest request)
        {
            var response = new MatriculaResponse();

            using (var con = new SqlConnection(cadena))
            {
                using (var cmd = new SqlCommand("uspInsertarMatriculaAlumno", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_alumno", request.IdAlumno);
                    cmd.Parameters.AddWithValue("@id_carrera", request.IdCarrera);
                    cmd.Parameters.AddWithValue("@id_curso", request.IdCurso);
                    cmd.Parameters.AddWithValue("@id_seccion", request.IdSeccion);
                    cmd.Parameters.AddWithValue("@id_periodo", request.IdPeriodo);

                    var resultadoParam = new SqlParameter("@resultado", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };

                    var mensajeParam = new SqlParameter("@mensaje", SqlDbType.VarChar, 200)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(resultadoParam);
                    cmd.Parameters.Add(mensajeParam);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    response.Resultado = Convert.ToBoolean(resultadoParam.Value);
                    response.Mensaje = mensajeParam.Value.ToString();
                }
            }

            return response;
        }

        public MatriculaResponse EliminarMatriculaAlumno(MatriculaDeleteRequest request)
        {
            var response = new MatriculaResponse();

            using (var con = new SqlConnection(cadena))
            {
                using (var cmd = new SqlCommand("uspEliminarMatriculaAlumno", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_alumno", request.IdAlumno);
                    cmd.Parameters.AddWithValue("@id_seccion", request.IdSeccion);
                    cmd.Parameters.AddWithValue("@id_periodo", request.IdPeriodo);

                    var resultadoParam = new SqlParameter("@resultado", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };

                    var mensajeParam = new SqlParameter("@mensaje", SqlDbType.VarChar, 200)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(resultadoParam);
                    cmd.Parameters.Add(mensajeParam);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    response.Resultado = Convert.ToBoolean(resultadoParam.Value);
                    response.Mensaje = mensajeParam.Value.ToString();
                }
            }

            return response;
        }

        public bool VerificarMatriculaAlumno(int idAlumno, int idSeccion, int idPeriodo)
        {
            using (var con = new SqlConnection(cadena))
            {
                using (var cmd = new SqlCommand("uspVerificarMatriculaAlumno", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_alumno", idAlumno);
                    cmd.Parameters.AddWithValue("@id_seccion", idSeccion);
                    cmd.Parameters.AddWithValue("@id_periodo", idPeriodo);

                    var existeParam = new SqlParameter("@existe", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(existeParam);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    return Convert.ToBoolean(existeParam.Value);
                }
            }
        }
    }
}
