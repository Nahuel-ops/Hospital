using BackendHospital.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendHospital.Data
{
    public class BaseDeDatos : DbContext
    {
        public BaseDeDatos(DbContextOptions<BaseDeDatos> options) : base(options)
        {
        }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
