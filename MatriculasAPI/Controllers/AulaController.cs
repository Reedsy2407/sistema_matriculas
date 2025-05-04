using MatriculasAPI.Repository.DAO;
using MatriculasMODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatriculasAPI.Controllers
{
    [Route("Aula")]
    [ApiController]
    public class AulaController : ControllerBase
    {
        //AULAS

        [HttpGet("ListadoAulas")]
        public async Task<ActionResult<List<Aula>>> ListadoAulas()
        {
            var lista = await Task.Run(() => new AulaDAO().aAulas());
            return Ok(lista);
        }

        [HttpPost("RegistrarAulas")]
        public async Task<ActionResult<bool>> RegistrarAulas(Aula objA)
        {
            var mensaje = await Task.Run(() => new AulaDAO().registrarAula(objA));
            return Ok(mensaje);
        }

        [HttpGet("BuscarAulas/{cod}")]
        public async Task<ActionResult<Aula>> BuscarAulas(string cod)
        {
            var aula = await Task.Run(() => new AulaDAO().buscarAula(cod));
            if (aula == null)
                return NotFound();
            return Ok(aula);
        }

        [HttpPut("ActualizarAulas")]
        public async Task<ActionResult<bool>> ActualizarAulas(Aula objA)
        {
            var resultado = await Task.Run(() => new AulaDAO().actualizarAula(objA));
            return Ok(resultado);
        }
    }
}
