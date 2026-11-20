using BackendHospital.Interfaces;
using ContratoHospital;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendHospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PacienteController : ControllerBase
    {
        private readonly IPaciente _pacienteService;

        public PacienteController(IPaciente pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_pacienteService.ObtenerTodos());
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var paciente = _pacienteService.ObtenerPorId(id);
            if (paciente == null)
            {
                return NotFound("El Id no se encuentra en paciente");
            }
            return Ok(paciente);
        }

        [HttpPost]
        public IActionResult Post(PacienteDTOs dto)
        {
            _pacienteService.Crear(dto);
            return Ok("Paciente registrado correctamente");
        }
    }
}
