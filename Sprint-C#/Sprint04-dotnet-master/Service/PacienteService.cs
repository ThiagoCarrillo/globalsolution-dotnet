using System.Collections.Generic;
using System.Threading.Tasks;
using Sessions_app.Models;
using Sessions_app.Data;
using AutoMapper;
using Sessions_app.Patterns; // Usaremos o AutoMapper para mapeamento de objetos

namespace Sessions_app.Services
{
    public class PacienteService
    {
        private readonly IPacienteRepository _repository;
        private readonly LoggerManager _logger = LoggerManager.GetInstance();

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Paciente>> GetAllPacientesAsync()
        {
            _logger.LogInfo("Serviço: Buscando todos os pacientes");
            return await _repository.GetAllAsync();
        }

        public async Task<Paciente> GetPacienteByIdAsync(int id)
        {
            _logger.LogInfo($"Serviço: Buscando paciente ID: {id}");
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Paciente> CreatePacienteAsync(Paciente paciente)
        {
            _logger.LogInfo($"Serviço: Criando paciente: {paciente.Nome}");
            return await _repository.CreateAsync(paciente);
        }

        public async Task<Paciente> UpdatePacienteAsync(Paciente paciente)
        {
            _logger.LogInfo($"Serviço: Atualizando paciente ID: {paciente.IdPaciente}");
            return await _repository.UpdateAsync(paciente);
        }

        public async Task<bool> DeletePacienteAsync(int id)
        {
            _logger.LogInfo($"Serviço: Excluindo paciente ID: {id}");
            return await _repository.DeleteAsync(id);
        }
    }
}
