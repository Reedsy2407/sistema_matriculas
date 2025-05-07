using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MatriculasMODELS;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Matriculas.Controllers
{
    public class ClaseController : Controller
    {
        Uri direccion = new Uri("https://localhost:7117/Clase/");
        private readonly HttpClient httpClient;

        public ClaseController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = direccion;
        }

        public async Task<IActionResult> seleccionCarrera()
        {
            var periodoResponse = await httpClient.GetAsync("Periodo/ObtenerPeriodoActual");
            if (periodoResponse.IsSuccessStatusCode)
            {
                var content = await periodoResponse.Content.ReadAsStringAsync();
                var periodo = JsonConvert.DeserializeObject<Periodo>(content);
                ViewBag.CodigoPeriodoActual = periodo?.codigo_periodo;
            }
            else
            {
                ViewBag.CodigoPeriodoActual = null;
            }

            var idUsuario = int.Parse(Request.Cookies["id_usuario"]);
            var carrerasResponse = await httpClient.GetAsync($"Carrera/ListarPorUsuario/{idUsuario}");
            if (carrerasResponse.IsSuccessStatusCode)
            {
                var carrerasContent = await carrerasResponse.Content.ReadAsStringAsync();
                var carreras = JsonConvert.DeserializeObject<List<Carrera>>(carrerasContent);

                ViewBag.Carreras = new SelectList(carreras, "id_carrera", "nom_carrera");
            }
            else
            {
                ViewBag.Carreras = new SelectList(new List<Carrera>(), "id_carrera", "nom_carrera");
            }

            return View();
        }

        public async Task<IActionResult> seleccionarCursos()
        {
            if (!Request.Cookies.ContainsKey("id_carrera") || !Request.Cookies.ContainsKey("id_usuario"))
            {
                TempData["ErrorMessage"] = "Debe seleccionar una carrera primero";
                return RedirectToAction("seleccionCarrera");
            }

            var idCarrera = int.Parse(Request.Cookies["id_carrera"]);

            try
            {
                var response = await httpClient.GetAsync($"Curso/ListarPorCarrera/{idCarrera}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var cursos = JsonConvert.DeserializeObject<List<Curso>>(content);

                    var carreraResponse = await httpClient.GetAsync($"https://localhost:7117/Carrera/BuscarCarrera/{idCarrera}");
                    var nombreCarrera = "Carrera seleccionada";

                    if (carreraResponse.IsSuccessStatusCode)
                    {
                        var carreraContent = await carreraResponse.Content.ReadAsStringAsync();
                        var carrera = JsonConvert.DeserializeObject<Carrera>(carreraContent);
                        nombreCarrera = carrera.nom_carrera;
                    }

                    ViewBag.Cursos = cursos;
                    ViewBag.CarreraSeleccionada = nombreCarrera;
                    ViewBag.TotalCreditos = cursos.Sum(c => c.creditos_curso);
                }
                else
                {
                    ViewBag.ErrorMessage = "No se pudieron cargar los cursos";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error: {ex.Message}";
            }

            return View();
        }

        [HttpPost]
        public IActionResult ProcesarSeleccion(int idCarrera)
        {
            if (idCarrera <= 0)
            {
                TempData["ErrorMessage"] = "Debe seleccionar una carrera válida";
                return RedirectToAction("seleccionCarrera");
            }

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("id_carrera", idCarrera.ToString(), cookieOptions);

            return RedirectToAction("seleccionarCursos");
        }

    }
}