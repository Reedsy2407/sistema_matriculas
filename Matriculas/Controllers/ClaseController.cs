using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
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
                // Obtener ID de usuario
                var idUsuario = int.Parse(Request.Cookies["id_usuario"]);
                ViewBag.IdUsuario = idUsuario;

                // Obtener periodo actual
                var periodoResponse = await httpClient.GetAsync("Periodo/ObtenerPeriodoActual");
                if (periodoResponse.IsSuccessStatusCode)
                {
                    var periodoContent = await periodoResponse.Content.ReadAsStringAsync();
                    var periodo = JsonConvert.DeserializeObject<Periodo>(periodoContent);
                    ViewBag.CodigoPeriodoActual = periodo?.id_periodo;
                }

                idCarrera = idCarrera != 0 ? idCarrera : (TempData["idCarrera"] as int?) ?? 0;
                TempData["idCarrera"] = idCarrera;

                // Obtener horarios
                var response = await httpClient.GetAsync($"Horario/ListarHorarioPorCurso/{idCurso}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = "No se pudieron cargar los horarios";
                    return View(new List<HorarioPorCurso>());
                }

                var content = await response.Content.ReadAsStringAsync();
                var horarios = JsonConvert.DeserializeObject<List<HorarioPorCurso>>(content);

                if (horarios == null || !horarios.Any())
                {
                    ViewBag.ErrorMessage = "No se encontraron horarios para este curso";
                    return View(new List<HorarioPorCurso>());
                }

                // Verificar matrículas
                var matriculados = new Dictionary<int, bool>();
                foreach (var horario in horarios)
                {
                    var matriculaResponse = await httpClient.GetAsync(
                        $"Matricula/VerificarMatricula/{idUsuario}/{horario.id_seccion}/{ViewBag.CodigoPeriodoActual}");

                    if (matriculaResponse.IsSuccessStatusCode)
                    {
                        var matriculaContent = await matriculaResponse.Content.ReadAsStringAsync();
                        matriculados[horario.id_seccion] = JsonConvert.DeserializeObject<bool>(matriculaContent);
                    }
                }

                ViewBag.HorariosAgrupados = horarios.GroupBy(h => h.id_seccion)
                                                  .ToDictionary(g => g.Key, g => g.ToList());
                ViewBag.Matriculados = matriculados;
                ViewBag.NombreCurso = horarios.First().nom_curso;
                ViewBag.IdCarrera = idCarrera;
                ViewBag.IdCurso = idCurso;

                return View(horarios);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error: {ex.Message}";
                return View(new List<HorarioPorCurso>());
            }
        }
        [HttpPost]
        public async Task<IActionResult> MatricularseHorario(
            [FromForm] int idAlumno,
            [FromForm] int idCarrera,
            [FromForm] int idCurso,
            [FromForm] int idSeccion,
            [FromForm] int idPeriodo)
        {
            try
            {
                var matriculaData = new
                {
                    idAlumno,
                    idCarrera,
                    idCurso,
                    idSeccion,
                    idPeriodo
                };

                var json = JsonConvert.SerializeObject(matriculaData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("Matricula/MatricularseHorario", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserializar la respuesta
                var result = JsonConvert.DeserializeObject<dynamic>(responseContent);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    // Validación de negocio (p.ej. ya matriculado en otro horario)
                    TempData["InfoMessage"] = result.mensaje.ToString();
                }
                else if (response.IsSuccessStatusCode && result.resultado == true)
                {
                    // Matrícula registrada exitosamente
                    TempData["SuccessMessage"] = result.mensaje.ToString();
                }
                else
                {
                    // Cualquier otro error (500, etc.)
                    TempData["ErrorMessage"] = "Ocurrió un error al procesar la matrícula";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error interno: {ex.Message}";
            }

            return RedirectToAction("seleccionarHorarios", new { idCurso, idCarrera });
        }

        [HttpPost("EliminarMatriculaPost")]
        public async Task<IActionResult> EliminarMatriculaPost(
        [FromForm] int idAlumno,
        [FromForm] int idSeccion,
        [FromForm] int idPeriodo,
        [FromForm] int idCurso,
        [FromForm] int idCarrera)
        {
            try
            {
                // Serializa con camelCase para que coincida con tu DTO MatriculaDeleteRequest
                var deletePayload = new
                {
                    idAlumno = idAlumno,
                    idSeccion = idSeccion,
                    idPeriodo = idPeriodo
                };
                var json = JsonConvert.SerializeObject(deletePayload);
                Console.WriteLine("JSON Enviado: " + json);

                // Construye la petición DELETE con body
                var request = new HttpRequestMessage(HttpMethod.Delete, "Matricula/EliminarMatricula")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };

                var response = await httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Respuesta del servidor: " + responseContent);
                var result = JsonConvert.DeserializeObject<dynamic>(responseContent);



                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    // Validación de negocio (p.ej. no existe la matrícula)
                    TempData["InfoMessage"] = result.mensaje.ToString();
                }
                else if (response.IsSuccessStatusCode && result.resultado == true)
                {
                    // Eliminación exitosa
                    TempData["SuccessMessage"] = result.mensaje.ToString();
                }
                else
                {
                    // Cualquier otro error de servidor
                    TempData["ErrorMessage"] = "Ocurrió un error al procesar la eliminación";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error interno: " + ex);
                TempData["ErrorMessage"] = $"Error interno: {ex.Message}";
            }

            return RedirectToAction("seleccionarHorarios", new { idCurso, idCarrera });
        }


    }
}