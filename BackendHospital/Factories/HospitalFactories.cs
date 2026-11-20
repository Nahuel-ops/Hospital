using BackendHospital.Data;
using BackendHospital.Models;
using Bogus;

namespace BackendHospital.Factories
{
    public static class HospitalFactories
    {
        public static void Seed(BaseDeDatos context)
        {
            Randomizer.Seed = new Random(12345);

            var especialidades = new[]
            {
                "Medicina General", "Cardiología", "Pediatría", "Traumatología",
                "Dermatología", "Neurología", "Ginecología", "Oftalmología"
            };

            var enfermedades = new[]
            {
                "Ninguna", "Hipertensión Arterial", "Diabetes Tipo 2",
                "Asma Bronquial", "Gastritis Crónica", "Alergia Estacional", "Migraña"
            };

            var motivosCita = new[]
            {
                "Control médico general", "Dolor de cabeza persistente", "Chequeo de rutina anual",
                "Seguimiento de tratamiento", "Revisión de resultados de laboratorio",
                "Malestar estomacal", "Control de presión arterial"
            };

            // 1. Sembrar 10 Médicos
            if (!context.Medicos.Any())
            {
                var fakerMedico = new Faker<Medico>("es")
                    .RuleFor(m => m.Id, f => Guid.NewGuid())
                    .RuleFor(m => m.Nombre, f => f.Name.FirstName())
                    .RuleFor(m => m.Apellido, f => f.Name.LastName())
                    .RuleFor(m => m.Telefono, f => f.Random.Int(60000000, 79999999))
                    .RuleFor(m => m.Especialidad, f => f.PickRandom(especialidades));

                var medicos = fakerMedico.Generate(10);
                context.Medicos.AddRange(medicos);
                context.SaveChanges();
            }

            // 2. Sembrar 50 Pacientes
            if (!context.Pacientes.Any())
            {
                var fakerPaciente = new Faker<Paciente>("es")
                    .RuleFor(p => p.Id, f => Guid.NewGuid())
                    .RuleFor(p => p.Nombre, f => f.Name.FirstName())
                    .RuleFor(p => p.Apellido, f => f.Name.LastName())
                    .RuleFor(p => p.Edad, f => f.Random.Int(1, 85))
                    .RuleFor(p => p.EnfermedadesBase, f => f.PickRandom(enfermedades));

                var pacientes = fakerPaciente.Generate(50);
                context.Pacientes.AddRange(pacientes);
                context.SaveChanges();
            }

            // 3. Sembrar 50 Citas enlazando Pacientes y Médicos existentes
            if (!context.Citas.Any())
            {
                var listaPacientes = context.Pacientes.ToList();
                var listaMedicos = context.Medicos.ToList();

                if (listaPacientes.Any() && listaMedicos.Any())
                {
                    var fakerCita = new Faker<Cita>("es")
                        .RuleFor(c => c.Id, f => Guid.NewGuid())
                        .RuleFor(c => c.Fecha, f => DateOnly.FromDateTime(f.Date.Between(DateTime.Now.AddDays(-15), DateTime.Now.AddDays(30))))
                        .RuleFor(c => c.Hora, f => new TimeOnly(f.Random.Int(8, 18), f.PickRandom(new[] { 0, 15, 30, 45 })))
                        .RuleFor(c => c.Comentario, f => f.PickRandom(motivosCita))
                        .RuleFor(c => c.IdPaciente, f => f.PickRandom(listaPacientes).Id)
                        .RuleFor(c => c.IdMedico, f => f.PickRandom(listaMedicos).Id);

                    var citas = fakerCita.Generate(50);
                    context.Citas.AddRange(citas);
                    context.SaveChanges();
                }
            }
        }
    }
}
