using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace ClienteMvc.Controllers
{
    public class RegionController : Controller
    {
        private readonly IHttpClientFactory _httpClient;

        public RegionController(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var cliente = _httpClient.CreateClient("ApiServicio");
            var respuesta = await cliente.GetAsync("api/region");

            if (!respuesta.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var json = await respuesta.Content.ReadAsStringAsync();
            var regiones = JsonDocument.Parse(json).RootElement;
            return View(regiones);
        }

        
        public async Task<IActionResult> Comunas(int idRegion)
        {
            var cliente = _httpClient.CreateClient("ApiServicio");
            var respuesta = await cliente.GetAsync($"api/region/{idRegion}/comunas");

            //if (!respuesta.IsSuccessStatusCode)
            //    return View("Error");

            var json = await respuesta.Content.ReadAsStringAsync();
            ViewBag.IdRegion = idRegion;
            var comunas = JsonDocument.Parse(json).RootElement;
            return View(comunas);
        }

        public async Task<IActionResult> Editar(int idRegion, int idComuna)
        {
            var cliente = _httpClient.CreateClient("ApiServicio");
            var respuesta = await cliente.GetAsync($"api/region/{idRegion}/comuna/{idComuna}");

            if (!respuesta.IsSuccessStatusCode)
                return NotFound();

            var json = await respuesta.Content.ReadAsStringAsync();
            ViewBag.IdRegion = idRegion;
            var comuna = JsonDocument.Parse(json).RootElement;
            return View(comuna);
        }

        
        [HttpPost]
        public async Task<IActionResult> Editar(int idRegion, int idComuna, IFormCollection datos)
        {
            var cliente = _httpClient.CreateClient("ApiServicio");

            var objeto = new
            {
                IdComuna = idComuna,
                Nombre = datos["Nombre"],
                IdRegion = idRegion,
                InformacionAdicional = datos["InformacionAdicional"]
            };

            var json = JsonSerializer.Serialize(objeto);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");

            var respuesta = await cliente.PostAsync($"api/region/{idRegion}/comuna", contenido);

            if (respuesta.IsSuccessStatusCode)
                return RedirectToAction("Comunas", new { idRegion });

            ViewBag.Error = "No se pudo guardar";
            return View(datos);
        }
    }
}
