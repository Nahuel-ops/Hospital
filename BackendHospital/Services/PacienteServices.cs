using BackendHospital.Data;
using BackendHospital.Interfaces;
using BackendHospital.Models;
using ContratoHospital;

namespace BackendHospital.Services
{
    public class PacienteServices : IPaciente
    {
        private readonly BaseDeDatos _context;

        public PacienteServices(BaseDeDatos context)
        {
            _context = context;
        }

        public List<Paciente> ObtenerTodos()
        {
            return _context.Pacientes.ToList();
        }

        public Paciente? ObtenerPorId(Guid id)
        {
            return _context.Pacientes.FirstOrDefault(p => p.Id == id);
        }

        public void Crear(PacienteDTOs dto)
        {
            var paciente = new Paciente
            {
                Id = Guid.NewGuid(),
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Edad = dto.Edad,
                EnfermedadesBase = dto.EnfermedadesBase
            };

            _context.Pacientes.Add(paciente);
            _context.SaveChanges();
        }
    }
}
