using Microsoft.EntityFrameworkCore;
using Sessions_app.Models;
using Sessions_app.Patterns;

namespace Sessions_app.Repositories
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly DataContext _context;
        private readonly LoggerManager _logger = LoggerManager.GetInstance();

        public MedicoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Medico>> GetAllAsync()
        {
            _logger.LogInfo("Buscando todos os médicos");
            return (IEnumerable<Medico>)await _context.Medicos.ToListAsync();
        }

        public async Task<Medico> GetByIdAsync(int id)
        {
            _logger.LogInfo($"Buscando médico com ID: {id}");
            return await _context.Medicos.FindAsync(id);
        }

        public async Task<Medico> CreateAsync(Medico medico)
        {
            await _context.Medicos.AddAsync(medico);
            await _context.SaveChangesAsync();
            return medico;
        }

        public async Task<Medico> UpdateAsync(Medico medico)
        {
            _logger.LogInfo($"Atualizando médico ID: {medico.IdMedico}");
            _context.Entry(medico).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return medico;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInfo($"Excluindo médico ID: {id}");
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
                return false;

            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
