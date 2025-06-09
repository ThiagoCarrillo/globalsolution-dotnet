using Microsoft.EntityFrameworkCore;
using Sessions_app.Models;
using Sessions_app.Patterns;

namespace Sessions_app.Repositories
{
    public class AgendamentoRepository : IAgendamentoRepository
    {
        private readonly DataContext _context;
        private readonly LoggerManager _logger = LoggerManager.GetInstance();

        public AgendamentoRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Agendamento>> GetAllAsync()
        {
            _logger.LogInfo("Buscando todos os agendamentos");
            return (IEnumerable<Agendamento>)await _context.Agendamentos
                .Include(a => a.Paciente)
                .Include(a => a.Medico)
                .ToListAsync();
        }

        public async Task<Agendamento> GetByIdAsync(int id)
        {
            _logger.LogInfo($"Buscando agendamento com ID: {id}");
            return await _context.Agendamentos
                .Include(a => a.Paciente)
                .Include(a => a.Medico)
                .FirstOrDefaultAsync(a => a.IdAgendamento == id);
        }

        public async Task<Agendamento> CreateAsync(Agendamento agendamento)
        {
            _logger.LogInfo($"Criando novo agendamento para {agendamento.DataAgendamento}");
            await _context.Agendamentos.AddAsync(agendamento);
            await _context.SaveChangesAsync();
            return agendamento;
        }

        public async Task<Agendamento> UpdateAsync(Agendamento agendamento)
        {
            _logger.LogInfo($"Atualizando agendamento ID: {agendamento.IdAgendamento}");
            _context.Entry(agendamento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return agendamento;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInfo($"Excluindo agendamento ID: {id}");
            var agendamento = await _context.Agendamentos.FindAsync(id);

            if (agendamento == null)
                return false;

            _context.Agendamentos.Remove(agendamento);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Agendamentos.AnyAsync(a => a.IdAgendamento == id);
        }
    }
}