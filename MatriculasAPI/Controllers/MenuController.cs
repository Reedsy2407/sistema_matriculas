using MatriculasAPI.Repository.DAO;
using MatriculasMODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MatriculasAPI.Controllers
{
    [Route("Menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        [HttpGet("ListarMenusPorRol/{idRol}")]
        public async Task<ActionResult<IEnumerable<Menu>>> ListarMenusPorRol(int idRol)
        {
            var menus = await Task.Run(() => new MenuDAO().listarMenusPorRol(idRol));
            return Ok(menus);
        }
    }
}
