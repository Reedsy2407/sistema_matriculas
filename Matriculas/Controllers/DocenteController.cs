using Microsoft.AspNetCore.Mvc;
using MatriculasMODELS;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace Matriculas.Controllers
{
    public class DocenteController : Controller
    {

        Uri direccion = new Uri("https://localhost:7117/Docente");
        private readonly HttpClient httpClient;

        public DocenteController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = direccion;
        }

        public List<Docente> aDocente()
        {
            List<Docente> lista = new List<Docente>();
            HttpResponseMessage response = httpClient.GetAsync(httpClient.BaseAddress + "/ListadoDocentes").Result;
            var data = response.Content.ReadAsStringAsync().Result;
            lista = JsonConvert.DeserializeObject<List<Docente>>(data);
            return lista;
        }

        public List<DocenteO> aDocenteO()
        {
            List<DocenteO> lista = new List<DocenteO>();
            HttpResponseMessage response = httpClient.GetAsync(httpClient.BaseAddress + "/ListadoDocentesO").Result;
            var data = response.Content.ReadAsStringAsync().Result;
            lista = JsonConvert.DeserializeObject<List<DocenteO>>(data);
            return lista;
        }

        public List<Especialidad> aEspecialidad()
        {
            List<Especialidad> lista = new List<Especialidad>();
            HttpResponseMessage response = httpClient.GetAsync(httpClient.BaseAddress + "/ListadoEspecialidades").Result;
            var data = response.Content.ReadAsStringAsync().Result;
            lista = JsonConvert.DeserializeObject<List<Especialidad>>(data);
            return lista;
        }

        [HttpGet]
        public IActionResult nuevoDocente()
        {
            ViewBag.Especialidad = new SelectList(aEspecialidad(), "cod_especialidad", "nom_especialidad");
            return View(new DocenteO());
        }

        [HttpPost]
        public async Task<IActionResult> nuevoDocente(DocenteO objD)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Especialidad = new SelectList(aEspecialidad(), "cod_especialidad", "nom_especialidad");
                return View(objD);
            }

            var json = JsonConvert.SerializeObject(objD);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/RegistrarDocentes", content);

            ViewBag.Mensaje = response.IsSuccessStatusCode
                ? "Docente registrado correctamente"
                : "Error al registrar docente";

            ViewBag.Especialidad = new SelectList(aEspecialidad(), "cod_especialidad", "nom_especialidad");
            return View(objD);
        }

        [HttpGet]
        public async Task<IActionResult> editarDocente(int id)
        {
            var response = await httpClient.GetAsync(
                httpClient.BaseAddress + "/BuscarDocentes/" + id
            );

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Especialidad = new SelectList(aEspecialidad(), "cod_especialidad", "nom_especialidad");
                ViewBag.Mensaje = "No se encontró el docente con ID " + id;
                return View(new DocenteO());
            }

            var content = await response.Content.ReadAsStringAsync();
            var objD = JsonConvert.DeserializeObject<DocenteO>(content)
                      ?? new DocenteO { id_docente = id };


            ViewBag.Especialidad = new SelectList(aEspecialidad(), "cod_especialidad", "nom_especialidad");
            return View(objD);
        }

        [HttpPost]
        public async Task<IActionResult> editarDocente(DocenteO objD)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Especialidad = new SelectList(aEspecialidad(), "cod_especialidad", "nom_especialidad");
                return View(objD);
            }

            var json = JsonConvert.SerializeObject(objD);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("/ActualizarDocentes", content);

            ViewBag.Mensaje = response.IsSuccessStatusCode
                ? "Docente modificado correctamente"
                : "Error al modificar docente";

            ViewBag.Especialidad = new SelectList(aEspecialidad(), "cod_especialidad", "nom_especialidad");
            return View(objD);
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult listadoDocentes()
        {
            return View(aDocente());
        }
    }
}
