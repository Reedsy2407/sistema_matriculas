using MatriculasAPI.Repository.DAO;
using MatriculasMODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatriculasAPI.Controllers
{
    [Route("Especialidad")]
    [ApiController]
    public class EspecialidadController : ControllerBase
    {
        // ESPECIALIDADES
        [HttpGet("ListadoEspecialidades")]
        public async Task<ActionResult<List<Especialidad>>> ListadoEspecialidad()
        {
            var lista = await Task.Run(() => new EspecialidadDAO().aEspecialidad());
            return Ok(lista);
        }
    }
}
