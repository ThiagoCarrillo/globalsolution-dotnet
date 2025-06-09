using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace Sessions_app.Models
{
    [Table("AGENDAMENTO")]
    [SwaggerSchema(Description = "Representa um agendamento médico no sistema")]
    public class Agendamento
    {
        [Key]
        [Column("ID_AGENDAMENTO")]
        [SwaggerSchema(Description = "Identificador único do agendamento")]
        public int IdAgendamento { get; set; }

        [Required(ErrorMessage = "O paciente é obrigatório")]
        [Column("PACIENTE_ID")]
        [SwaggerSchema(Description = "ID do paciente relacionado")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "O médico é obrigatório")]
        [Column("MEDICO_ID")]
        [SwaggerSchema(Description = "ID do médico relacionado")]
        public int MedicoId { get; set; }

        [Required(ErrorMessage = "A data/hora é obrigatória")]
        [Column("DATA_AGENDAMENTO")]
        [Display(Name = "Data/Hora do Agendamento")]
        [SwaggerSchema(Description = "Data e hora do agendamento")]
        public DateTime DataAgendamento { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "Máximo de 500 caracteres")]
        [Column("DESCRICAO")]
        [Display(Name = "Descrição")]
        [SwaggerSchema(Description = "Detalhes do agendamento")]
        public string Descricao { get; set; }


        [ForeignKey("PacienteId")]
        [SwaggerSchema(Description = "Paciente relacionado")]
        public virtual Paciente? Paciente { get; set; }

        [ForeignKey("MedicoId")]
        [SwaggerSchema(Description = "Médico relacionado")]
        public virtual Medico? Medico { get; set; }
    }
}