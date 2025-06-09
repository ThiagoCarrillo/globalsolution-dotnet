using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sessions_app.Models;
using Sessions_app.Patterns;
using Sessions_app.Service;
using Sessions_app.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Sessions_app.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly AgendamentoService _service;
        private readonly PacienteService _pacienteService;
        private readonly MedicoService _medicoService;
        private readonly LoggerManager _logger = LoggerManager.GetInstance();

        public AgendamentoController(
            AgendamentoService service,
            PacienteService pacienteService,
            MedicoService medicoService)
        {
            _service = service;
            _pacienteService = pacienteService;
            _medicoService = medicoService;
        }

        /// <summary>
        /// Lista todos os agendamentos cadastrados
        /// </summary>
        /// <returns>Uma view com a lista de agendamentos</returns>
        /// <response code="200">Retorna a página com a lista de agendamentos</response>
        public async Task<IActionResult> Index()
        {
            _logger.LogInfo("Controller MVC: Listando todos os agendamentos");
            var agendamentos = await _service.GetAllAgendamentosAsync();
            return View(agendamentos);
        }

        /// <summary>
        /// Exibe os detalhes de um agendamento específico
        /// </summary>
        /// <param name="id">ID do agendamento</param>
        /// <returns>Uma view com os detalhes do agendamento</returns>
        /// <response code="200">Retorna os detalhes do agendamento solicitado</response>
        /// <response code="404">Se o agendamento não for encontrado</response>
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInfo($"Controller MVC: Exibindo detalhes do agendamento ID: {id}");
            var agendamento = await _service.GetAgendamentoByIdAsync(id);

            if (agendamento == null)
            {
                _logger.LogWarning($"Agendamento com ID {id} não encontrado");
                return NotFound();
            }

            return View(agendamento);
        }

        /// <summary>
        /// Exibe o formulário para criar um novo agendamento
        /// </summary>
        /// <returns>Uma view com o formulário de criação</returns>
        /// <response code="200">Retorna o formulário para criação de agendamento</response>
        public async Task<IActionResult> Create()
        {
            _logger.LogInfo("Controller MVC: Exibindo formulário para criar agendamento");
            await PopulateViewData();
            return View();
        }

        /// <summary>
        /// Cria um novo agendamento no sistema
        /// </summary>
        /// <param name="agendamento">Dados do agendamento a ser criado</param>
        /// <returns>Redireciona para a lista de agendamentos em caso de sucesso</returns>
        /// <response code="200">Retorna o formulário com erros de validação</response>
        /// <response code="302">Redireciona para a lista após criação bem-sucedida</response>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Agendamento agendamento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInfo($"Controller MVC: Criando agendamento para {agendamento.DataAgendamento}");
                    await _service.CreateAgendamentoAsync(agendamento);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao criar agendamento: {ex.Message}");
                    ModelState.AddModelError("", ex.Message);
                }
            }
            await PopulateViewData();
            return View(agendamento);
        }

        /// <summary>
        /// Exibe o formulário para editar um agendamento existente
        /// </summary>
        /// <param name="id">ID do agendamento a ser editado</param>
        /// <returns>Uma view com o formulário de edição</returns>
        /// <response code="200">Retorna o formulário para edição</response>
        /// <response code="404">Se o agendamento não for encontrado</response>
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInfo($"Controller MVC: Exibindo formulário para editar agendamento ID: {id}");
            var agendamento = await _service.GetAgendamentoByIdAsync(id);

            if (agendamento == null)
            {
                _logger.LogWarning($"Agendamento com ID {id} não encontrado");
                return NotFound();
            }

            await PopulateViewData();
            return View(agendamento);
        }

        /// <summary>
        /// Atualiza os dados de um agendamento existente
        /// </summary>
        /// <param name="id">ID do agendamento</param>
        /// <param name="agendamento">Dados atualizados do agendamento</param>
        /// <returns>Redireciona para a lista em caso de sucesso</returns>
        /// <response code="200">Retorna o formulário com erros</response>
        /// <response code="302">Redireciona após atualização bem-sucedida</response>
        /// <response code="404">Se o ID não corresponder ou agendamento não existir</response>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Agendamento agendamento)
        {
            if (id != agendamento.IdAgendamento)
            {
                _logger.LogWarning($"ID {id} não corresponde ao ID do agendamento {agendamento.IdAgendamento}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInfo($"Controller MVC: Atualizando agendamento ID: {id}");
                    await _service.UpdateAgendamentoAsync(agendamento);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao atualizar agendamento: {ex.Message}");
                    ModelState.AddModelError("", ex.Message);
                }
            }
            await PopulateViewData();
            return View(agendamento);
        }

        /// <summary>
        /// Exibe a confirmação para excluir um agendamento
        /// </summary>
        /// <param name="id">ID do agendamento a ser excluído</param>
        /// <returns>Uma view de confirmação</returns>
        /// <response code="200">Retorna a confirmação</response>
        /// <response code="404">Se o agendamento não for encontrado</response>
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInfo($"Controller MVC: Exibindo confirmação para excluir agendamento ID: {id}");
            var agendamento = await _service.GetAgendamentoByIdAsync(id);

            if (agendamento == null)
            {
                _logger.LogWarning($"Agendamento com ID {id} não encontrado");
                return NotFound();
            }

            return View(agendamento);
        }

        /// <summary>
        /// Exclui um agendamento do sistema
        /// </summary>
        /// <param name="id">ID do agendamento a ser excluído</param>
        /// <returns>Redireciona para a lista após exclusão</returns>
        /// <response code="302">Redireciona após exclusão bem-sucedida</response>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInfo($"Controller MVC: Excluindo agendamento ID: {id}");
            await _service.DeleteAgendamentoAsync(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Exibe página de erro quando um agendamento não é encontrado
        /// </summary>
        /// <param name="id">ID do agendamento não encontrado</param>
        /// <returns>Uma view com mensagem de erro</returns>
        /// <response code="200">Retorna a página de erro</response>
        public IActionResult ErroAgendamentoNaoEncontrado(int id)
        {
            ViewData["AgendamentoId"] = id;
            return View();
        }

        private async Task PopulateViewData()
        {
            ViewData["PacienteId"] = new SelectList(
                await _pacienteService.GetAllPacientesAsync(),
                "IdPaciente",
                "Nome");

            ViewData["MedicoId"] = new SelectList(
                await _medicoService.GetAllMedicosAsync(),
                "IdMedico",
                "Nome");
        }
    }
}