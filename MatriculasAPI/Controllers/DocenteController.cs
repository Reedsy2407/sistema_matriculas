using MatriculasAPI.Repository.DAO;
using MatriculasMODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatriculasAPI.Controllers
{
    [Route("Docente")]
    [ApiController]
    public class DocenteController : ControllerBase
    {
        // DOCENTES
        [HttpGet("ListadoDocentes")]
        public async Task<ActionResult<List<Docente>>> ListadoDocentes()
        {
            var lista = await Task.Run(() => new DocenteDAO().aDocentes());
            return Ok(lista);
        }

        [HttpGet("ListadoDocentesO")]
        public async Task<ActionResult<List<DocenteO>>> ListadoDocentesO()
        {
            var lista = await Task.Run(() => new DocenteDAO().aDocentesO());
            return Ok(lista);
        }

        [HttpPost("RegistrarDocentes")]
        public async Task<ActionResult<bool>> ListadoDocentes(DocenteO objD)
        {
            var mensaje = await Task.Run(() => new DocenteDAO().registrarDocente(objD));
            return Ok(mensaje);
        }

        [HttpGet("BuscarDocentes/{id}")]
        public async Task<ActionResult<Docente>> BuscarDocentes(int id)
        {
            var docente = await Task.Run(() => new DocenteDAO().buscarDocente(id));
            if (docente == null)
                return NotFound();
            return Ok(docente);
        }

        [HttpPut("ActualizarDocentes")]
        public async Task<ActionResult<bool>> ActualizarDocentes(DocenteO objD)
        {
            var resultado = await Task.Run(() => new DocenteDAO().actualizarDocente(objD));
            return Ok(resultado);
        }
    }
}
