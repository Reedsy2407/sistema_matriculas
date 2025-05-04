using MatriculasMODELS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace Matriculas.Controllers
{
    public class CursoController : Controller
    {
        Uri direccion = new Uri("https://localhost:7117/Curso");
        private readonly HttpClient httpClient;

        public CursoController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = direccion;
        }

        public List<Curso> aCurso()
        {
            List<Curso> lista = new List<Curso>();
            HttpResponseMessage response = httpClient.GetAsync(httpClient.BaseAddress + "/ListadoCursos").Result;
            var data = response.Content.ReadAsStringAsync().Result;
            lista = JsonConvert.DeserializeObject<List<Curso>>(data);
            return lista;
        }

        public List<Curso> aCursoApi()
        {
            HttpResponseMessage response = httpClient.GetAsync(httpClient.BaseAddress + "/ListadoCursos").Result;
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
            var response = await httpClient.PostAsync("/RegistrarCursos", content);

            ViewBag.Mensaje = response.IsSuccessStatusCode
                ? "Curso registrado correctamente"
                : "Error al registrar curso";

            ViewBag.Carreras = new SelectList(aCarreras(), "Value", "Text");
            return View(objC);
        }

        [HttpGet]
        public async Task<IActionResult> editarCurso(int id)
        {
            var response = await httpClient.GetAsync(
                httpClient.BaseAddress + "/BuscarCursos/" + id
            );

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
            var response = await httpClient.PutAsync(httpClient.BaseAddress + "/ActualizarCursos", content);
            ViewBag.Mensaje = response.IsSuccessStatusCode
                ? "Curso modificado correctamente"
                : "Error al modificar curso";
            ViewBag.Carreras = new SelectList(
                aCarreras(), "Value", "Text", objC.id_carrera
            );

            return View(objC);
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
