using Microsoft.AspNetCore.Mvc;
using AccesoDatos; 

namespace ApiServicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly RegionServicio _servicio;

        public RegionController(RegionServicio servicio)
        {
            _servicio = servicio;
        }

        
        [HttpGet]
        public IActionResult GetTodasRegiones()
        {
            var regiones = _servicio.ObtenerTodasRegiones();
            return Ok(regiones); 
        }

        [HttpGet("{idRegion}")]
        public IActionResult GetRegionPorId(int idRegion)
        {
            // Validación Region Válida
            if (idRegion <= 0)
                return BadRequest("El ID de región no es válido"); // 400

            var region = _servicio.ObtenerRegionPorId(idRegion);
            if (region == null)
                return NotFound("Región no encontrada"); // 404

            return Ok(region);
        }

        [HttpGet("{idRegion}/comuna")]
        public IActionResult GetComunasDeRegion(int idRegion)
        {
            var comuna = _servicio.ObtenerComunasPorRegion(idRegion);
            if (idRegion <= 0)
                return BadRequest("ID de región inválido");

            return Ok(comuna);
        }

        [HttpGet("{idRegion}/comuna/{idComuna}")]
        public IActionResult GetComunaEspecifica(int idRegion, int idComuna)
        {
            var comuna = _servicio.ObtenerComuna(idComuna);
            if (idComuna <= 0)
                return BadRequest("ID de Comuna inválida");

            return Ok(comuna);
        }

        
        [HttpPost("{idRegion}/comuna")]
        public IActionResult GuardarComuna(int idRegion, [FromBody] Comuna comuna)
        {
            _servicio.EjecutarMerge(comuna);
            if (idRegion <= 0 || comuna == null)
                return BadRequest("Datos inválidos");

            return Ok("Comuna guardada/actualizada correctamente");
        }
    }
}
