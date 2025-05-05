using MatriculasAPI.Repository.DAO;
using MatriculasMODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatriculasAPI.Controllers
{
    [Route("Alumno")]
    [ApiController]
    public class AlumnoController : ControllerBase
    {

        [HttpGet("ListadoAlumnos")]
        public async Task<ActionResult<List<Alumno>>> ListadoAlumnos()
        {
            var lista = await Task.Run(() => new AlumnoDAO().aAlumnos());
            return Ok(lista);
        }

        [HttpGet("BuscarAlumno/{id}")]
        public async Task<ActionResult<AlumnoO>> BuscarAlumno(int id)
        {
            var alumno = await Task.Run(() => new AlumnoDAO().buscarAlumno(id));
            return alumno == null ? NotFound() : Ok(alumno);
        }

        [HttpPost("RegistrarAlumno")]
        public async Task<ActionResult<bool>> RegistrarAlumno(AlumnoO objA)
        {
            var resultado = await Task.Run(() => new AlumnoDAO().registrarAlumno(objA));
            return Ok(resultado);
        }

        [HttpPut("ActualizarAlumno")]
        public async Task<ActionResult<bool>> ActualizarAlumno(AlumnoO objA)
        {
            var resultado = await Task.Run(() => new AlumnoDAO().actualizarAlumno(objA));
            return Ok(resultado);
        }

    }
}
