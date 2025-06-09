using System.Collections.Generic;
using System.Threading.Tasks;
using Sessions_app.Models;

namespace Sessions_app.Data
{
    public interface IPacienteRepository
    {
        Task<IEnumerable<Paciente>> GetAllAsync();
        Task<Paciente> GetByIdAsync(int id);
        Task<Paciente> CreateAsync(Paciente paciente);
        Task<Paciente> UpdateAsync(Paciente paciente);
        Task<bool> DeleteAsync(int id);
    }
}
