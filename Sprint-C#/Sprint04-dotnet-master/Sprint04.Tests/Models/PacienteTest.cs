using Sessions_app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint01.Tests.Models
{
    public class PacienteTests
    {
        private Paciente CreateValidPaciente()
        {
            return new Paciente
            {
                Nome = "Maria Silva",
                Telefone = "(11) 98888-7777",
                Email = "maria.silva@example.com",
                DataNascimento = new DateTime(1990, 5, 15)
            };
        }

        [Fact]
        public void Nome_QuandoAusente_DeveRetornarErro()
        {
            // Arrange
            var paciente = CreateValidPaciente();
            paciente.Nome = null;

            // Act
            var results = ValidateModel(paciente);

            // Assert
            Assert.False(results.IsValid);
            Assert.Contains(results.ErrorMessages, error => error.Contains("nome é obrigatório"));
        }

        [Fact]
        public void Nome_ComMaisDe255Caracteres_DeveRetornarErro()
        {
            // Arrange
            var paciente = CreateValidPaciente();
            paciente.Nome = new string('A', 256); // 256 caracteres

            // Act
            var results = ValidateModel(paciente);

            // Assert
            Assert.False(results.IsValid);
            Assert.Contains(results.ErrorMessages, error => error.Contains("máximo 255 caracteres"));
        }

        [Fact]
        public void Email_FormatoInvalido_DeveRetornarErro()
        {
            // Arrange
            var paciente = CreateValidPaciente();
            paciente.Email = "email-invalido";

            // Act
            var results = ValidateModel(paciente);

            // Assert
            Assert.False(results.IsValid);
            Assert.Contains(results.ErrorMessages, error => error.Contains("e-mail inválido"));
        }

        [Fact]
        public void Email_ComMaisDe255Caracteres_DeveRetornarErro()
        {
            // Arrange
            var paciente = CreateValidPaciente();
            paciente.Email = new string('a', 245) + "@example.com"; // 256 caracteres

            // Act
            var results = ValidateModel(paciente);

            // Assert
            Assert.False(results.IsValid);
            Assert.Contains(results.ErrorMessages, error => error.Contains("email deve ter no máximo 255 caracteres"));
        }

        [Fact]
        public void Telefone_QuandoAusente_DeveRetornarErro()
        {
            // Arrange
            var paciente = CreateValidPaciente();
            paciente.Telefone = null;

            // Act
            var results = ValidateModel(paciente);

            // Assert
            Assert.False(results.IsValid);
            Assert.Contains(results.ErrorMessages, error => error.Contains("telefone é obrigatório"));
        }

        [Fact]
        public void Telefone_ComMaisDe15Caracteres_DeveRetornarErro()
        {
            // Arrange
            var paciente = CreateValidPaciente();
            paciente.Telefone = new string('9', 16); // 16 caracteres

            // Act
            var results = ValidateModel(paciente);

            // Assert
            Assert.False(results.IsValid);
            Assert.Contains(results.ErrorMessages, error => error.Contains("telefone deve ter no máximo 15 caracteres"));
        }

        [Fact]
        public void DataNascimento_ValidaSeFormatacaoCorreta()
        {
            // Arrange
            var paciente = CreateValidPaciente();

            // Act & Assert
            // Este teste apenas verifica se um DateTime pode ser atribuído à propriedade DataNascimento
            Assert.IsType<DateTime>(paciente.DataNascimento);
        }

        private (bool IsValid, List<string> ErrorMessages) ValidateModel(Paciente model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(model, context, results, true);
            var errorMessages = results.Select(r => r.ErrorMessage).ToList();
            return (isValid, errorMessages);
        }
    }
}
