using MatriculasAPI.Models;
using MatriculasAPI.Repository.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatriculasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculaController : ControllerBase
    {
        [HttpGet("ListadoCursos")]
        public async Task<ActionResult<List<Curso>>> ListadoCursos()
        {
            var lista = await Task.Run(() => new CursoDAO().aCursos());
            return Ok(lista);
        }

        [HttpPost("RegistrarCursos")]
        public async Task<ActionResult<bool>> ListadoCursos(Curso objC)
        {
            var mensaje = await Task.Run(() => new CursoDAO().registrarCurso(objC));
            return Ok(mensaje);
        }
    }
}
