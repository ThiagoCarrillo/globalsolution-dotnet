using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sessions_app.Models
{
    [Table("PACIENTE")]
    [SwaggerSchema(Description = "Entidade que representa um paciente no sistema")]
    public class Paciente
    {
        /// <summary>
        /// Identificador único do paciente
        /// </summary>
        /// <example>1</example>
        [Key]
        [Column("ID_PACIENTE")]
        [SwaggerSchema(Description = "Identificador único do paciente")]
        public int IdPaciente { get; set; }

        /// <summary>
        /// Nome completo do paciente
        /// </summary>
        /// <example>João da Silva</example>
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres")]
        [Column("NOME")]
        [SwaggerSchema(Description = "Nome completo do paciente")]
        public string Nome { get; set; }

        /// <summary>
        /// Número de telefone para contato
        /// </summary>
        /// <example>(11) 98765-4321</example>
        [Required(ErrorMessage = "O telefone é obrigatório")]
        [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres")]
        [Column("TELEFONE")]
        [SwaggerSchema(Description = "Número de telefone do paciente")]
        public string Telefone { get; set; }

        /// <summary>
        /// Endereço de e-mail do paciente
        /// </summary>
        /// <example>joao.silva@exemplo.com</example>
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
        [StringLength(255, ErrorMessage = "O email deve ter no máximo 255 caracteres")]
        [Column("EMAIL")]
        [SwaggerSchema(Description = "Endereço de e-mail do paciente")]
        public string Email { get; set; }

        /// <summary>
        /// Data de nascimento do paciente
        /// </summary>
        /// <example>1985-07-15</example>
        [DataType(DataType.Date)]
        [Column("DATA_NASCIMENTO")]
        [SwaggerSchema(Description = "Data de nascimento do paciente", Format = "date")]
        public DateTime? DataNascimento { get; set; }
    }
}
