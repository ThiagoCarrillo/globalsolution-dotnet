using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace Sessions_app.Models
{
    [Table("MEDICO")]
    [SwaggerSchema(Description = "Representa um médico no sistema")]
    public class Medico
    {
        [Key]
        [Column("ID_MEDICO")]
        [Description("Identificador único do médico")]
        public int IdMedico { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres")]
        [Column("NOME")]
        [Description("Nome completo do médico")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres")]
        [Column("TELEFONE")]
        [Description("Número de telefone do médico")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
        [StringLength(255, ErrorMessage = "O email deve ter no máximo 255 caracteres")]
        [Column("EMAIL")]
        [Description("Endereço de e-mail do médico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O CRM é obrigatório")]
        [StringLength(15, ErrorMessage = "O CRM deve ter no máximo 15 caracteres")]
        [Column("CRM")]
        [Description("Número do CRM do médico")]
        public string CRM { get; set; }
    }
}