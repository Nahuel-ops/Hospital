using ContratoHospital;

namespace BackendHospital.Interfaces
{
    public interface IAuthInterface
    {
        void Registrar(LoginDto dto);
        LoginRespuestaDTO? Login(LoginDto dto);
    }
}
