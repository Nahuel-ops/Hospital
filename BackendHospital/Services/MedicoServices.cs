using BackendHospital.Data;
using BackendHospital.Interfaces;
using BackendHospital.Models;
using ContratoHospital;

namespace BackendHospital.Services
{
    public class MedicoServices : IMedico
    {
        private readonly BaseDeDatos _context;

        public MedicoServices(BaseDeDatos context)
        {
            _context = context;
        }

        public List<Medico> ObtenerTodos()
        {
            return _context.Medicos.ToList();
        }

        public Medico? ObtenerPorId(Guid id)
        {
            return _context.Medicos.FirstOrDefault(m => m.Id == id);
        }

        public void Crear(MedicoDTOs dto)
        {
            var medico = new Medico
            {
                Id = Guid.NewGuid(),
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Telefono = dto.Telefono,
                Especialidad = dto.Especialidad
            };

            _context.Medicos.Add(medico);
            _context.SaveChanges();
        }
    }
}
