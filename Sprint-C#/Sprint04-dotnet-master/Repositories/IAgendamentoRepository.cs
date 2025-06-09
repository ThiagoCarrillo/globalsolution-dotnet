using Sessions_app.Models;

namespace Sessions_app.Repositories
{
    public interface IAgendamentoRepository
    {
        Task<IEnumerable<Agendamento>> GetAllAsync();
        Task<Agendamento> GetByIdAsync(int id);
        Task<Agendamento> CreateAsync(Agendamento agendamento);
        Task<Agendamento> UpdateAsync(Agendamento agendamento);
        Task<bool> DeleteAsync(int id);
    }
}
