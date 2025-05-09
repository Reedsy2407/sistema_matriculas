using MatriculasAPI.Repository.DAO;
using MatriculasMODELS;
using MatriculasMODELS.Matricula;
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

        [HttpGet("Carrera/ListarPorUsuario/{idUsuario}")]
        public async Task<ActionResult<List<Carrera>>> GetCarrerasPorUsuario(int idUsuario)
        {
            var carreras = await Task.Run(() => new ClaseDAO().listarCarrerasPorUsuario(idUsuario));
            return Ok(carreras);
        }

        [HttpGet("Curso/ListarPorCarrera/{idCarrera}")]
        public async Task<ActionResult<List<Curso>>> GetCursosPorCarrera(int idCarrera)
        {
            var cursos = await Task.Run(() => new ClaseDAO().listarCursosPorCarrera(idCarrera));

            if (cursos == null || !cursos.Any())
                return NotFound("No se encontraron cursos para la carrera especificada");

            return Ok(cursos);
        }

        [HttpGet("Horario/ListarHorarioPorCurso/{idCurso}")]
        public async Task<ActionResult<List<HorarioPorCurso>>> GetHorariosPorCurso(int idCurso)
        {
            var horarios = await Task.Run(() => new ClaseDAO().ListarHorariosPorCurso(idCurso));

            if (horarios == null || !horarios.Any())
                return NotFound("No se encontraron horarios para el curso seleccionado");

            return Ok(horarios); 
        }

        [HttpPost("Matricula/MatricularseHorario")]
        public IActionResult InsertarMatricula([FromBody] MatriculaRequest request)
        {
            var response = new ClaseDAO().InsertarMatriculaAlumno(request);

            if (response.Resultado)
                return Ok(response);
            else
                return BadRequest(response);
        }

        // En MatriculasAPI.Controllers.ClaseController
        [HttpDelete("Matricula/EliminarMatricula")]
        public IActionResult EliminarMatricula([FromBody] MatriculaDeleteRequest request)
        {
            var response = new ClaseDAO().EliminarMatriculaAlumno(request);

            if (response.Resultado)
                return Ok(response);
            else
                return BadRequest(response);
        }

        // En MatriculasAPI.Controllers.ClaseController
        [HttpGet("Matricula/VerificarMatricula/{idAlumno}/{idSeccion}/{idPeriodo}")]
        public async Task<ActionResult<bool>> VerificarMatricula(int idAlumno, int idSeccion, int idPeriodo)
        {
            try
            {
                var estaMatriculado = await Task.Run(() =>
                    new ClaseDAO().VerificarMatriculaAlumno(idAlumno, idSeccion, idPeriodo));

                return Ok(estaMatriculado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al verificar matrícula: {ex.Message}");
            }
        }

    }
}
