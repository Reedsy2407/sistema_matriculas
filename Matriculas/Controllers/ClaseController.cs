using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MatriculasMODELS;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

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

        public async Task<IActionResult> SeleccionCarrera()
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

        [HttpPost]
        public IActionResult procesarCarrera(int idCarrera)
        {
            if (idCarrera <= 0)
            {
                TempData["ErrorMessage"] = "Debe seleccionar una carrera válida";
                return RedirectToAction("SeleccionCarrera");
            }

            return RedirectToAction("seleccionarCursos", new { idCarrera });
        }

        public async Task<IActionResult> seleccionarCursos(int idCarrera)
        {
            if (!Request.Cookies.ContainsKey("id_usuario"))
            {
                TempData["ErrorMessage"] = "Debe iniciar sesión primero";
                return RedirectToAction("SeleccionCarrera");
            }

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
                    ViewBag.IdCarrera = idCarrera;
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
        public IActionResult procesarCurso(int idCurso, int idCarrera)
        {
            if (idCurso <= 0)
            {
                TempData["ErrorMessage"] = "Debe seleccionar un curso válido";
                return RedirectToAction("seleccionarCursos", new { idCarrera });
            }

            TempData["idCarrera"] = idCarrera;
            return RedirectToAction("seleccionarHorarios", new { idCurso, idCarrera });
        }


        [HttpGet]
        public async Task<IActionResult> seleccionarHorarios(int idCurso, int idCarrera)
        {
            try
            {

                var periodoResponse = await httpClient.GetAsync("Periodo/ObtenerPeriodoActual");
                if (periodoResponse.IsSuccessStatusCode)
                {
                    var periodoContent = await periodoResponse.Content.ReadAsStringAsync();
                    var periodo = JsonConvert.DeserializeObject<Periodo>(periodoContent);
                    ViewBag.CodigoPeriodoActual = periodo?.id_periodo;
                }
                idCarrera = idCarrera != 0 ? idCarrera : (TempData["idCarrera"] as int?) ?? 0;
                TempData["idCarrera"] = idCarrera; 

                var response = await httpClient.GetAsync($"Horario/ListarHorarioPorCurso/{idCurso}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var horarios = JsonConvert.DeserializeObject<List<HorarioPorCurso>>(content);

                    if (horarios == null || !horarios.Any())
                    {
                        ViewBag.ErrorMessage = "No se encontraron horarios para este curso";
                        return View(new List<HorarioPorCurso>());
                    }

                    var horariosAgrupados = horarios.GroupBy(h => h.id_seccion)
                                                  .ToDictionary(g => g.Key, g => g.ToList());

                    ViewBag.HorariosAgrupados = horariosAgrupados;
                    ViewBag.NombreCurso = horarios.First().nom_curso;
                    ViewBag.IdCarrera = idCarrera;
                    ViewBag.IdCurso = idCurso;

                    return View(horarios);
                }
                else
                {
                    ViewBag.ErrorMessage = "No se pudieron cargar los horarios";
                    return View(new List<HorarioPorCurso>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error: {ex.Message}";
                return View(new List<HorarioPorCurso>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> MatricularseHorario(
     int id_carrera,
     int id_curso,
     int id_seccion,
     int id_periodo)
        {
            try
            {
                // Leer el ID del alumno desde la cookie
                int id_alumno = int.Parse(Request.Cookies["id_usuario"]);
                Console.WriteLine($"Enrollment attempt: Student ID: {id_alumno}, Career: {id_carrera}, Course: {id_curso}, Section: {id_seccion}, Period: {id_periodo}");

                var matriculaData = new
                {
                    id_alumno,
                    id_carrera,
                    id_curso,
                    id_seccion,
                    id_periodo
                };

                var json = JsonConvert.SerializeObject(matriculaData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("Matricula/MatricularseHorario", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var responseObj = JsonConvert.DeserializeObject<dynamic>(result);

                    TempData["SuccessMessage"] = (responseObj.resultado == true)
                        ? responseObj.mensaje
                        : responseObj.mensaje;
                }
                else
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Error al conectar con el servicio de matrícula. Detalles: {errorBody}";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error interno: {ex.Message}";
            }

            return RedirectToAction("seleccionarHorarios", new { idCurso = id_curso, id_carrera });
        }

    }
}