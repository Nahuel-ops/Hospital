using BackendHospital.Models;
using ContratoHospital;

namespace BackendHospital.Interfaces
{
    public interface ICita
    {
        List<Cita> ObtenerTodos();
        Cita? ObtenerPorId(Guid id);
        void Crear(CitaDTOs dto);
    }
}
