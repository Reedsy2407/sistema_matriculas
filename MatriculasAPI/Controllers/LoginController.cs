using MatriculasAPI.Repository.DAO;
using MatriculasMODELS.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatriculasAPI.Controllers
{
    [Route("Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<LoginResponse>> login([FromBody] LoginRequest request)
        {
            var respuesta = await Task.Run(() => new LoginDAO().login(request));

            if (!respuesta.es_exito)
                return Unauthorized(respuesta);

            return Ok(respuesta);
        }
    }
}
