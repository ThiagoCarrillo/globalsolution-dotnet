using Sessions_app.Models;
using Sessions_app.Repositories;
using Sessions_app.Patterns;
using System.ComponentModel.DataAnnotations;

namespace Sessions_app.Service
{
    public class AgendamentoService
    {
        private readonly IAgendamentoRepository _repository;
        private readonly LoggerManager _logger = LoggerManager.GetInstance();

        public AgendamentoService(IAgendamentoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Agendamento>> GetAllAgendamentosAsync()
        {
            _logger.LogInfo("Service: Obtendo todos os agendamentos");
            return await _repository.GetAllAsync();
        }

        public async Task<Agendamento> GetAgendamentoByIdAsync(int id)
        {
            _logger.LogInfo($"Service: Buscando agendamento ID: {id}");
            var agendamento = await _repository.GetByIdAsync(id);

            if (agendamento == null)
            {
                _logger.LogWarning($"Service: Agendamento ID {id} não encontrado");
                throw new KeyNotFoundException("Agendamento não encontrado");
            }

            return agendamento;
        }

        public async Task<Agendamento> CreateAgendamentoAsync(Agendamento agendamento)
        {
            try
            {
                _logger.LogInfo("Service: Criando novo agendamento");
                await ValidateAgendamentoAsync(agendamento);
                return await _repository.CreateAsync(agendamento);
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Service: Erro de validação - {ex.Message}");
                throw;
            }
        }

        public async Task<Agendamento> UpdateAgendamentoAsync(Agendamento agendamento)
        {
            try
            {
                _logger.LogInfo($"Service: Atualizando agendamento ID: {agendamento.IdAgendamento}");
                await ValidateAgendamentoAsync(agendamento);
                return await _repository.UpdateAsync(agendamento);
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Service: Erro na atualização - {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAgendamentoAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private async Task ValidateAgendamentoAsync(Agendamento agendamento)
        {
            var validationContext = new ValidationContext(agendamento);
            Validator.ValidateObject(agendamento, validationContext, true);

            // Validação de conflito de horário
            var hasConflict = await _repository.GetAllAsync()
                .ContinueWith(task => task.Result.Any(a =>
                    a.IdAgendamento != agendamento.IdAgendamento &&
                    a.MedicoId == agendamento.MedicoId &&
                    a.DataAgendamento == agendamento.DataAgendamento));

            if (hasConflict)
            {
                throw new ValidationException("Médico já possui outro agendamento neste mesmo horário");
            }
        }
    }
}