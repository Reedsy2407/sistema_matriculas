using MatriculasAPI.Repository.DAO;
using MatriculasMODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatriculasAPI.Controllers
{
    [Route("Carrera")]
    [ApiController]
    public class CarreraController : ControllerBase
    {
        [HttpGet("BuscarCarrera/{id}")]
        public async Task<ActionResult<Carrera>> BuscarCarrera(int id)
        {
            var carrera = await Task.Run(() => new ClaseDAO().buscarCarreraPorId(id));

            if (carrera == null)
                return NotFound($"No se encontró la carrera con ID {id}");

            return Ok(carrera);

        }

        [HttpGet("ListadoCarreras")]
        public async Task<ActionResult<List<Carrera>>> ListadoCarreras()
        {
            CarreraDAO dao = new CarreraDAO();
            List<Carrera> lista = await Task.Run(() => new List<Carrera>(dao.listarCarreras()));
            return Ok(lista);
        }

        [HttpGet("ListadoCarrerasPorUsuario/{idUsuario}")]
        public async Task<ActionResult<List<Carrera>>> ListadoCarrerasPorUsuario(int idUsuario)
        {
            CarreraDAO dao = new CarreraDAO();
            List<Carrera> lista = await Task.Run(() => new List<Carrera>(dao.listarCarrerasPorUsuario(idUsuario)));
            return Ok(lista);
        }
    }
}
