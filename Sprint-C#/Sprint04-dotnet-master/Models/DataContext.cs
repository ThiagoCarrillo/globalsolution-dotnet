using Microsoft.EntityFrameworkCore;

namespace Sessions_app.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Paciente> Pacientes { get; set; }

        public DbSet<Medico> Medicos { get; set; }

        public DbSet<Agendamento> Agendamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração específica para Oracle
            modelBuilder.HasDefaultSchema("RM553565");

            // Configuração para sequências Oracle
            modelBuilder.Entity<Paciente>()
                .Property(p => p.IdPaciente)
                .HasDefaultValueSql("PACIENTE_SEQ.NEXTVAL");

            modelBuilder.Entity<Medico>()
                .Property(m => m.IdMedico)
                .HasDefaultValueSql("MEDICO_SEQ.NEXTVAL");
        }


    }
}
