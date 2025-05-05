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
    }
}
