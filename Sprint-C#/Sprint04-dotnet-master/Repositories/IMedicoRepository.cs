using Sessions_app.Models;

namespace Sessions_app.Repositories
{
    public interface IMedicoRepository
    {
        Task<IEnumerable<Medico>> GetAllAsync();
        Task<Medico> GetByIdAsync(int id);
        Task<Medico> CreateAsync(Medico medico);
        Task<Medico> UpdateAsync(Medico medico);
        Task<bool> DeleteAsync(int id);
    }
}
