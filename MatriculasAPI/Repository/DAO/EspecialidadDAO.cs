using System.Data.SqlClient;
using MatriculasAPI.Models;
using MatriculasAPI.Repository.Interfaces;

namespace MatriculasAPI.Repository.DAO
{
    public class EspecialidadDAO : IEspecialidad
    {

        private readonly string cadena = "";
        public EspecialidadDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }

        public IEnumerable<Especialidad> aEspecialidad()
        {
            List<Especialidad> lista = new List<Especialidad>();
            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_listarEspecialidad", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Especialidad
                {
                    cod_especialidad = int.Parse(dr[0].ToString()),
                    nom_especialidad = dr[1].ToString(),
                });
            }
            dr.Close();
            con.Close();
            cmd.Dispose();
            return lista;
        }
    }
}
