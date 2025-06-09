using Sessions_app.Models;
using Sessions_app.Patterns;
using Sessions_app.Repositories;

namespace Sessions_app.Service
{
    public class MedicoService
    {
        private readonly IMedicoRepository _repository;
        private readonly LoggerManager _logger = LoggerManager.GetInstance();
        public MedicoService(IMedicoRepository repository)
        {
            _repository = repository;
        }
        public async Task<Medico> GetMedicoByIdAsync(int id)
        {
            _logger.LogInfo($"Serviço: Buscando médico ID: {id}");
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Medico> CreateMedicoAsync(Medico medico)
        {
            _logger.LogInfo($"Serviço: Criando médico: {medico.Nome}");
            return await _repository.CreateAsync(medico);
        }

        public async Task<Medico> UpdateMedicoAsync(Medico medico)
        {
            _logger.LogInfo($"Serviço: Atualizando médico ID: {medico.IdMedico}");
            return await _repository.UpdateAsync(medico);
        }

        public async Task<bool> DeleteMedicoAsync(int id)
        {
            _logger.LogInfo($"Serviço: Excluindo médico ID: {id}");
            return await _repository.DeleteAsync(id);
        }
        public async Task<IEnumerable<Medico>> GetAllMedicosAsync()
        {
            _logger.LogInfo("Serviço: Buscando todos os pacientes");
            return await _repository.GetAllAsync();
        }

    }
}
