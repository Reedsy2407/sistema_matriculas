using MatriculasMODELS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System.Diagnostics;

namespace Matriculas.Controllers
{
    public class MatriculaController : Controller
    {
        private readonly HttpClient httpClient;

        public MatriculaController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:7117/api/Matricula/");
        }

        public async Task<IActionResult> listadoMatricula()
        {
            try
            {
                var idUsuario = int.Parse(Request.Cookies["id_usuario"]);

                var response = await httpClient.GetAsync($"ListadoMatricula/{idUsuario}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var lista = JsonConvert.DeserializeObject<List<MatriculasMODELS.Matriculas>>(json);
                    return View(lista);
                }
                else
                {
                    ViewBag.Error = "No se pudo cargar la matrícula.";
                    return View(new List<MatriculasMODELS.Matriculas>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return View(new List<MatriculasMODELS.Matriculas>());
            }
        }

        public async Task<IActionResult> ExportarMatricula()
        {
            try
            {
                var idUsuario = int.Parse(Request.Cookies["id_usuario"]);
                var response = await httpClient.GetAsync($"ListadoMatricula/{idUsuario}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var lista = JsonConvert.DeserializeObject<List<MatriculasMODELS.Matriculas>>(json);

                    DateTime hoy = DateTime.Now;
                    return new ViewAsPdf("GenerarPDF", lista)
                    {
                        FileName = $"Relacion_Matricula_{hoy:yyyyMMdd}.pdf",
                        PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                        PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    };
                }
                else
                {
                    return Content("No se pudo generar el PDF.");
                }
            }
            catch (Exception ex)
            {
                return Content("Error al generar PDF: " + ex.Message);
            }
        }

    }

}

