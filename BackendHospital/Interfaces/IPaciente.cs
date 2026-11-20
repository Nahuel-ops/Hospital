using BackendHospital.Models;
using ContratoHospital;

namespace BackendHospital.Interfaces
{
    public interface IPaciente
    {
        List<Paciente> ObtenerTodos();
        Paciente? ObtenerPorId(Guid id);
        void Crear(PacienteDTOs dto);
    }
}
