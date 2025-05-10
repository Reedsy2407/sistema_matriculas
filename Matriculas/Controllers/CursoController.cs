using MatriculasMODELS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Matriculas.Controllers
{
    public class CursoController : Controller
    {
        private readonly HttpClient httpClient;

        public CursoController()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7117/")
            };
        }


        public List<Curso> aCurso()
        {
            List<Curso> lista = new List<Curso>();
            HttpResponseMessage response = httpClient.GetAsync("Curso/ListadoCursos").Result;
            var data = response.Content.ReadAsStringAsync().Result;
            lista = JsonConvert.DeserializeObject<List<Curso>>(data);
            return lista;
        }

        public List<Curso> aCursoApi()
        {
            HttpResponseMessage response = httpClient.GetAsync("Curso/ListadoCursos").Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Curso>>(data)!;
        }

        public List<SelectListItem> aCarreras()
        {
            return aCursoApi()
                .Select(c => new { c.id_carrera, c.nom_carrera })
                .Distinct()
                .Select(x => new SelectListItem
                {
                    Value = x.id_carrera.ToString(),
                    Text = x.nom_carrera
                })
                .ToList();
        }

        [HttpGet]
        public IActionResult registrarCurso()
        {
            ViewBag.Carreras = new SelectList(aCarreras(), "Value", "Text");
            return View(new CursoO());
        }

        [HttpPost]
        public async Task<IActionResult> registrarCurso(CursoO objC)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Carreras = new SelectList(aCarreras(), "Value", "Text");
                return View(objC);
            }

            var json = JsonConvert.SerializeObject(objC);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Curso/RegistrarCursos", content);

            ViewBag.Mensaje = response.IsSuccessStatusCode
                ? "Curso registrado correctamente"
                : "Error al registrar curso";

            ViewBag.Carreras = new SelectList(aCarreras(), "Value", "Text");
            return View(objC);
        }

        [HttpGet]
        public async Task<IActionResult> editarCurso(int id)
        {
            var response = await httpClient.GetAsync($"Curso/BuscarCursos/{id}");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Mensaje = "No se encontró el curso con ID " + id;
                ViewBag.Carreras = new SelectList(aCarreras(), "Value", "Text");
                return View(new CursoO());
            }

            var content = await response.Content.ReadAsStringAsync();

            var api = JsonConvert
                .DeserializeObject<Curso>(content)
                ?? new Curso { id_curso = id };

            var curso = new CursoO
            {
                id_curso = api.id_curso,
                nom_curso = api.nom_curso,
                creditos_curso = api.creditos_curso,
                id_carrera = api.id_carrera
            };

            ViewBag.Carreras = new SelectList(
                aCarreras(), "Value", "Text", curso.id_carrera
            );
            return View(curso);
        }


        [HttpPost]
        public async Task<IActionResult> editarCurso(CursoO objC)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Carreras = new SelectList(aCarreras(), "Value", "Text", objC.id_carrera);
                return View(objC);
            }
            var json = JsonConvert.SerializeObject(objC);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("Curso/ActualizarCursos", content);

            ViewBag.Mensaje = response.IsSuccessStatusCode
                ? "Curso modificado correctamente"
                : "Error al modificar curso";
            ViewBag.Carreras = new SelectList(
                aCarreras(), "Value", "Text", objC.id_carrera
            );

            return View(objC);
        }

        [HttpGet]
        public IActionResult AsignarHorarioCurso(int idCurso)
        {
            // Rellenar todos los ViewBag necesarios
            CargarViewBags(idCurso);

            // Crear modelo inicial
            var model = new HorarioCursoNuevo { id_curso = idCurso };
            return View(model);
        }


        [HttpPost]
        public IActionResult AsignarHorarioCurso(HorarioCursoNuevo horario)
        {
            if (horario.id_seccion == null && string.IsNullOrWhiteSpace(horario.cod_seccion))
            {
                ModelState.AddModelError(nameof(horario.id_seccion),
                    "Debes escoger una sección existente o indicar un código de sección nuevo.");
                ModelState.AddModelError(nameof(horario.cod_seccion), "");
            }

            if (!ModelState.IsValid)
            {
                CargarViewBags(horario.id_curso);
                return View(horario);
            }

            var json = JsonConvert.SerializeObject(horario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var resp = httpClient.PostAsync("Curso/AsignarHorarioCurso", content).Result;

            var jObj = JObject.Parse(resp.Content.ReadAsStringAsync().Result);
            TempData["InfoMessage"] = jObj["mensaje"]?.ToString() ?? "Respuesta inválida";

            return RedirectToAction("AsignarHorarioCurso", new { idCurso = horario.id_curso });
        }

        private void CargarViewBags(int idCurso)
        {
            // Cursos
            var cursosJson = httpClient.GetAsync("Curso/ListadoCursos").Result
                                    .Content.ReadAsStringAsync().Result;
            var cursos = JsonConvert.DeserializeObject<List<Curso>>(cursosJson) ?? new List<Curso>();
            // Nombre de curso
            var cursoActual = cursos.FirstOrDefault(c => c.id_curso == idCurso);
            ViewBag.CursoNombre = cursoActual?.nom_curso ?? "—";

            // Docentes
            var docJson = httpClient.GetAsync("Docente/ListadoDocentes").Result
                                   .Content.ReadAsStringAsync().Result;
            var docentes = JsonConvert.DeserializeObject<List<Docente>>(docJson) ?? new List<Docente>();
            ViewBag.Docentes = new SelectList(docentes, "id_docente", "nom_docente");

            // Aulas
            var aulaJson = httpClient.GetAsync("Aula/ListadoAulas").Result
                                   .Content.ReadAsStringAsync().Result;
            var aulas = JsonConvert.DeserializeObject<List<Aula>>(aulaJson) ?? new List<Aula>();
            ViewBag.Aulas = new SelectList(aulas, "id_aula", "cod_aula");

            // Secciones
            var secJson = httpClient.GetAsync($"Curso/SeccionesPorCurso/{idCurso}").Result
                                   .Content.ReadAsStringAsync().Result;
            var secciones = JsonConvert.DeserializeObject<List<Seccion>>(secJson) ?? new List<Seccion>();
            ViewBag.Secciones = new SelectList(secciones, "id_seccion", "cod_seccion");

            // Días
            ViewBag.Dias = new SelectList(new[]
            {
                new { Value = 1, Text = "Lunes" },
                new { Value = 2, Text = "Martes" },
                new { Value = 3, Text = "Miércoles" },
                new { Value = 4, Text = "Jueves" },
                new { Value = 5, Text = "Viernes" },
                new { Value = 6, Text = "Sábado" },
                new { Value = 7, Text = "Domingo" }
            }, "Value", "Text");
        }


        public IActionResult Index()
        {
            return View();
        }
    
        public IActionResult listadoCursos()
        {
            return View(aCurso());
        }
    }
}
