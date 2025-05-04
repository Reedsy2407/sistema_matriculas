using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatriculasMODELS.Login
{
    public class LoginRequest
    {
        [DisplayName("Correo electrónico")]
        public string correo { get; set; }

        [DisplayName("Contraseña")]
        public string contrasena { get; set; }
    }
}
