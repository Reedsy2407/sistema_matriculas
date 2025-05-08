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

        public IEnumerable<Matricula> aMatricula(int id_matricula)
        {
            List<Matricula> lista = new List<Matricula>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_listarMatricula", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_matricula", id_matricula);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Matricula
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
                            CodAula = dr["cod_aula"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
    }
}
