using MatriculasAPI.Repository.Interfaces;
using MatriculasMODELS;
using System.Data;
using System.Data.SqlClient;

namespace MatriculasAPI.Repository.DAO
{
    public class MenuDAO : IMenu
    {
        private readonly string cadena;

        public MenuDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("cn");
        }

        public bool asignarMenuARol(MenuRol menuRol)
        {
            bool exito = false;
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_asignarMenuARol", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_menu", menuRol.id_menu);
                cmd.Parameters.AddWithValue("@id_rol", menuRol.id_rol);

                con.Open();
                exito = cmd.ExecuteNonQuery() > 0;
            }
            return exito;
        }

        public IEnumerable<Menu> listarMenus()
        {
            List<Menu> lista = new List<Menu>();

            SqlConnection con = new SqlConnection(cadena);
            SqlCommand cmd = new SqlCommand("usp_listarMenus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Menu
                {
                    id_menu = Convert.ToInt32(dr["id_menu"]),
                    titulo_menu = dr["titulo_menu"].ToString(),
                    url_menu = dr["url_menu"].ToString(),
                    orden = Convert.ToInt32(dr["orden"])
                });
            }
            return lista.OrderBy(m => m);
        }

        public IEnumerable<Menu> listarMenusPorRol(int idRol)
        {
            List<Menu> menus = new List<Menu>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerMenusPorRol", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_rol", idRol);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    menus.Add(new Menu
                    {
                        id_menu = Convert.ToInt32(dr["id_menu"]),
                        titulo_menu = dr["titulo_menu"].ToString(),
                        url_menu = dr["url_menu"].ToString(),
                        controlador = dr["controlador"].ToString(),
                        orden = Convert.ToInt32(dr["orden"])
                    });
                }
            }

            return menus.OrderBy(m => m.orden).ToList();
        }

        public bool registrarMenu(Menu menu)
        {
            bool exito = false;
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("usp_registrarMenu", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@titulo_menu", menu.titulo_menu);
                cmd.Parameters.AddWithValue("@url_menu", menu.url_menu);
                cmd.Parameters.AddWithValue("@orden", menu.orden);

                con.Open();
                exito = cmd.ExecuteNonQuery() > 0;
            }
            return exito;
        }


    }
}
