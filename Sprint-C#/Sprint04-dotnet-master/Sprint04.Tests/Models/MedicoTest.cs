using Sessions_app.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Sprint01.Tests.Models
{
    public class MedicoTests
    {
        private Medico CreateValidMedico() => new Medico
        {
            Nome = "Dr. João Silva",
            Telefone = "(11) 99999-9999",
            Email = "joao.silva@example.com",
            CRM = "CRM/SP 123456"
        };

        private (bool IsValid, List<string> ErrorMessages) ValidateModel(Medico model)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);
            return (isValid, validationResults.Select(v => v.ErrorMessage).ToList());
        }

        [Theory]
        [InlineData(null, "Nome")]
        [InlineData("", "Nome")]
        [InlineData("   ", "Nome")]
        public void Nome_NaoPreenchido_DeveRetornarErro(string nome, string propertyName)
        {
            var medico = CreateValidMedico();
            medico.Nome = nome;

            var result = ValidateModel(medico);

            Assert.False(result.IsValid);
            Assert.Contains(result.ErrorMessages, e => e.Contains(propertyName) && e.Contains("obrigatório"));
        }

        [Fact]
        public void Nome_ExcedeTamanhoMaximo_DeveRetornarErro()
        {
            var medico = CreateValidMedico();
            medico.Nome = new string('A', 256);

            var result = ValidateModel(medico);

            Assert.False(result.IsValid);
            Assert.Contains(result.ErrorMessages, e => e.Contains("Nome") && e.Contains("máximo") && e.Contains("255"));
        }

        [Theory]
        [InlineData("email-invalido")]
        [InlineData("sem@dominio")]
        [InlineData("@semusuario.com")]
        public void Email_FormatoInvalido_DeveRetornarErro(string email)
        {
            var medico = CreateValidMedico();
            medico.Email = email;

            var result = ValidateModel(medico);

            Assert.False(result.IsValid);
            Assert.Contains(result.ErrorMessages, e => e.Contains("Email") && e.Contains("válido"));
        }

        [Theory]
        [InlineData(null, "Telefone")]
        [InlineData("", "Telefone")]
        [InlineData("   ", "Telefone")]
        public void Telefone_NaoPreenchido_DeveRetornarErro(string telefone, string propertyName)
        {
            var medico = CreateValidMedico();
            medico.Telefone = telefone;

            var result = ValidateModel(medico);

            Assert.False(result.IsValid);
            Assert.Contains(result.ErrorMessages, e => e.Contains(propertyName) && e.Contains("obrigatório"));
        }

        [Fact]
        public void Telefone_ExcedeTamanhoMaximo_DeveRetornarErro()
        {
            var medico = CreateValidMedico();
            medico.Telefone = new string('9', 16);

            var result = ValidateModel(medico);

            Assert.False(result.IsValid);
            Assert.Contains(result.ErrorMessages, e => e.Contains("Telefone") && e.Contains("máximo") && e.Contains("15"));
        }

        [Theory]
        [InlineData(null, "CRM")]
        [InlineData("", "CRM")]
        [InlineData("   ", "CRM")]
        public void CRM_NaoPreenchido_DeveRetornarErro(string crm, string propertyName)
        {
            var medico = CreateValidMedico();
            medico.CRM = crm;

            var result = ValidateModel(medico);

            Assert.False(result.IsValid);
            Assert.Contains(result.ErrorMessages, e => e.Contains(propertyName) && e.Contains("obrigatório"));
        }

        [Fact]
        public void CRM_ExcedeTamanhoMaximo_DeveRetornarErro()
        {
            var medico = CreateValidMedico();
            medico.CRM = new string('C', 16);

            var result = ValidateModel(medico);

            Assert.False(result.IsValid);
            Assert.Contains(result.ErrorMessages, e => e.Contains("CRM") && e.Contains("máximo") && e.Contains("15"));
        }
    }
}