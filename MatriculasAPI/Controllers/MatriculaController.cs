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

        [HttpGet("ListadoEspecialidades")]
        public async Task<ActionResult<List<Especialidad>>> ListadoEspecialidad()
        {
            var lista = await Task.Run(() => new EspecialidadDAO().aEspecialidad());
            return Ok(lista);
        }


    }
}
