namespace MatriculasMODELS.Login
{
    public class LoginResponse
    {
        public bool es_exito { get; set; }
        public string mensaje { get; set; }
        public int id_usuario { get; set; }
        public int id_rol { get; set; }
    }
}
