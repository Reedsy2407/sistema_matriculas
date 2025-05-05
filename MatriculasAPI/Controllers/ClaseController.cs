using MatriculasAPI.Repository.DAO;
using MatriculasMODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatriculasAPI.Controllers
{
    [Route("Clase")]
    [ApiController]
    public class ClaseController : ControllerBase
    {
        [HttpGet("Periodo/ObtenerPeriodoActual")]
        public async Task<ActionResult<Periodo>> BuscarPeriodoActual()
        {
            var periodo = await Task.Run(() => new ClaseDAO().BuscarPeriodoActual());

            if (periodo == null)
                return NotFound("No se encontró un período activo");

            return Ok(periodo);
        }
    }
}
