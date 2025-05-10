using MatriculasAPI.Repository.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MatriculasMODELS;

namespace MatriculasAPI.Controllers
{
    [Route("Curso")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        // CURSOS
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

        [HttpGet("BuscarCursos/{id}")]
        public async Task<ActionResult<Curso>> BuscarCursos(int id)
        {
            var curso = await Task.Run(() => new CursoDAO().buscarCurso(id));
            return Ok(curso);
        }

        [HttpPut("ActualizarCursos")]
        public async Task<ActionResult<List<Curso>>> ActualizarCursos(Curso objC)
        {
            var lista = await Task.Run(() => new CursoDAO().actualizarCurso(objC));
            return Ok(lista);
        }

        [HttpPost("AsignarHorarioCurso")]
        public ActionResult AsignarHorarioCurso([FromBody] HorarioCursoNuevo model)
        {
            var dao = new CursoDAO();
            if (dao.AsignarHorarioConProcedure(model, out string mensaje))
                return Ok(new { success = true, mensaje });
            else
                return BadRequest(new { success = false, mensaje });
        }

        [HttpGet("CursosSinHorario")]
        public ActionResult<List<Curso>> CursosSinHorario()
        {
            var lista = new CursoDAO().ListarCursosSinHorario();
            return Ok(lista);
        }

        [HttpGet("SeccionesPorCurso/{idCurso}")]
        public ActionResult<List<Seccion>> SeccionesPorCurso(int idCurso)
        {
            var lista = new CursoDAO().ListarSeccionesPorCurso(idCurso).ToList();
            return Ok(lista);
        }
    }
}
