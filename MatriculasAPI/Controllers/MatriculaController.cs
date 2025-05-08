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
        [HttpGet("ListadoMatricula")]
        public async Task<ActionResult<List<Matricula>>> ListadoMatricula(int id_matricula)
        {
            var lista = await Task.Run(() => new MatriculaDAO().aMatricula(id_matricula));
            return Ok(lista);
        }
    }
}
