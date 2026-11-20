using BackendHospital.Models;
using ContratoHospital;

namespace BackendHospital.Interfaces
{
    public interface IMedico
    {
        List<Medico> ObtenerTodos();
        Medico? ObtenerPorId(Guid id);
        void Crear(MedicoDTOs dto);
    }
}
