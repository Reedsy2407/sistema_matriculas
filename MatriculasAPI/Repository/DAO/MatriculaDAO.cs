using MatriculasAPI.Repository.Interfaces;
using MatriculasMODELS;
using System.Data.SqlClient;

namespace MatriculasAPI.Repository.DAO
{
    public class MatriculaDAO : IMatricula
    {
        private readonly string cadena;

        public MatriculaDAO()
        {
            cadena = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("cn");
        }

        public IEnumerable<Matriculas> aMatricula(int id_usuario)
        {
            List<Matriculas> lista = new List<Matriculas>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_listarMatricula", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_usuario", id_usuario);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Matriculas
                        {
                            IdMatricula = Convert.ToInt32(dr["id_matricula"]),
                            IdUsuario = Convert.ToInt32(dr["id_usuario"]),
                            NombreCompleto = dr["nombre_completo"].ToString(),
                            CodigoPeriodo = dr["codigo_periodo"].ToString(),
                            IdCarrera = Convert.ToInt32(dr["id_carrera"]),
                            NomCarrera = dr["nom_carrera"].ToString(),
                            IdCurso = Convert.ToInt32(dr["id_curso"]),
                            NomCurso = dr["nom_curso"].ToString(),
                            CreditosCurso = Convert.ToInt32(dr["creditos_curso"]),
                            IdSeccion = Convert.ToInt32(dr["id_seccion"]),
                            CodSeccion = dr["cod_seccion"].ToString(),
                            IdAula = Convert.ToInt32(dr["id_aula"]),
                            CodAula = dr["cod_aula"].ToString(),                           
                            horaInicio = (TimeSpan)dr["hora_inicio"],
                            horaFin = (TimeSpan)dr["hora_fin"],
                            tipoHorario = dr["tipo_horario"].ToString(),
                            nomDiaSemana = dr["nomDiaSemana"].ToString(),
                        });
                    }
                }
            }
            return lista;
        }
    }
}
