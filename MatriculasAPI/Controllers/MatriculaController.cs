using MatriculasAPI.Repository.DAO;
using MatriculasMODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatriculasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculaController : ControllerBase
    {
        [HttpGet("ListadoMatricula/{idUsuario}")]
        public async Task<ActionResult<List<Matriculas>>> ListadoMatricula(int idUsuario)
        {
            var lista = await Task.Run(() => new MatriculaDAO().aMatricula(idUsuario));
            return Ok(lista);
        }
    }
}
