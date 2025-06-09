using Microsoft.AspNetCore.Mvc;
using Sessions_app.Models;
using Sessions_app.Patterns;
using Sessions_app.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace Sessions_app.Controllers
{
    public class MedicoController : Controller
    {
        private readonly MedicoService _service;
        private readonly LoggerManager _logger = LoggerManager.GetInstance();
        public MedicoController(MedicoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lista todos os médicos cadastrados
        /// </summary>
        /// <returns>Uma view com a lista de todos os médicos</returns>
        /// <response code="200">Retorna a página com a lista de médicos</response>
        // GET: Medico
        public async Task<IActionResult> Index()
        {
            _logger.LogInfo("Controller MVC: Listando todos os médicos");
            var medicos = await _service.GetAllMedicosAsync();
            return View(medicos);
        }

        /// <summary>
        /// Exibe os detalhes de um médico específico
        /// </summary>
        /// <param name="id">ID do médico</param>
        /// <returns>Uma view com os detalhes do médico</returns>
        /// <response code="200">Retorna os detalhes do médico solicitado</response>
        /// <response code="404">Se o médico não for encontrado</response>
        // GET: Medico/Details/5
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInfo($"Controller MVC: Exibindo detalhes do médico ID: {id}");
            var medico = await _service.GetMedicoByIdAsync(id);
            if (medico == null)
            {
                _logger.LogWarning($"Médico com ID {id} não encontrado");
                return NotFound();
            }
            return View(medico);
        }

        /// <summary>
        /// Exibe o formulário para criar um novo médico
        /// </summary>
        /// <returns>Uma view com o formulário de criação</returns>
        /// <response code="200">Retorna o formulário para criação de médico</response>
        // GET: Medico/Create
        public IActionResult Create()
        {
            _logger.LogInfo("Controller MVC: Exibindo formulário para criar médico");
            return View();
        }

        /// <summary>
        /// Cria um novo médico no sistema
        /// </summary>
        /// <param name="medico">Dados do médico a ser criado</param>
        /// <returns>Redireciona para a lista de médicos em caso de sucesso</returns>
        /// <response code="200">Retorna o formulário com erros de validação</response>
        /// <response code="302">Redireciona para a lista de médicos após criação bem-sucedida</response>
        // POST: Medico/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medico medico)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInfo($"Controller MVC: Criando médico {medico.Nome}");
                await _service.CreateMedicoAsync(medico);
                return RedirectToAction(nameof(Index));
            }
            return View(medico);
        }

        /// <summary>
        /// Exibe o formulário para editar um médico existente
        /// </summary>
        /// <param name="id">ID do médico a ser editado</param>
        /// <returns>Uma view com o formulário de edição</returns>
        /// <response code="200">Retorna o formulário para edição do médico</response>
        /// <response code="404">Se o médico não for encontrado</response>
        // GET: Medico/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInfo($"Controller MVC: Exibindo formulário para editar médico ID: {id}");
            var medico = await _service.GetMedicoByIdAsync(id);
            if (medico == null)
            {
                _logger.LogWarning($"Médico com ID {id} não encontrado");
                return NotFound();
            }
            return View(medico);
        }

        /// <summary>
        /// Atualiza os dados de um médico existente
        /// </summary>
        /// <param name="id">ID do médico</param>
        /// <param name="medico">Dados atualizados do médico</param>
        /// <returns>Redireciona para a lista de médicos em caso de sucesso</returns>
        /// <response code="200">Retorna o formulário com erros de validação</response>
        /// <response code="302">Redireciona para a lista de médicos após atualização bem-sucedida</response>
        /// <response code="404">Se o ID na URL não corresponder ao ID do médico ou se o médico não for encontrado</response>
        // POST: Medico/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Medico medico)
        {
            if (id != medico.IdMedico)
            {
                _logger.LogWarning($"ID {id} não corresponde ao ID do médico {medico.IdMedico}");
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _logger.LogInfo($"Controller MVC: Atualizando médico ID: {id}");
                await _service.UpdateMedicoAsync(medico);
                return RedirectToAction(nameof(Index));
            }
            return View(medico);
        }

        /// <summary>
        /// Exibe a confirmação para excluir um médico
        /// </summary>
        /// <param name="id">ID do médico a ser excluído</param>
        /// <returns>Uma view com a confirmação de exclusão</returns>
        /// <response code="200">Retorna a confirmação para exclusão do médico</response>
        /// <response code="404">Se o médico não for encontrado</response>
        // GET: Medico/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInfo($"Controller MVC: Exibindo confirmação para excluir médico ID: {id}");
            var medico = await _service.GetMedicoByIdAsync(id);
            if (medico == null)
            {
                _logger.LogWarning($"Médico com ID {id} não encontrado");
                return NotFound();
            }
            return View(medico);
        }

        /// <summary>
        /// Exclui um médico do sistema
        /// </summary>
        /// <param name="id">ID do médico a ser excluído</param>
        /// <returns>Redireciona para a lista de médicos após a exclusão</returns>
        /// <response code="302">Redireciona para a lista de médicos após exclusão bem-sucedida</response>
        // POST: Medico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInfo($"Controller MVC: Excluindo médico ID: {id}");
            await _service.DeleteMedicoAsync(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Exibe página de erro quando um médico não é encontrado
        /// </summary>
        /// <param name="id">ID do médico não encontrado</param>
        /// <returns>Uma view com a mensagem de erro</returns>
        /// <response code="200">Retorna a página de erro</response>
        public IActionResult ErroMedicoNaoEncontrado(int id)
        {
            ViewData["MedicoId"] = id;
            return View();
        }

        /// <summary>
        /// Lista as especialidades médicas disponíveis
        /// </summary>
        /// <returns>Uma view com as especialidades médicas</returns>
        /// <response code="200">Retorna a página com as especialidades médicas</response>
        [HttpGet]
        public IActionResult EspecialidadesMedicas()
        {
            return View();
        }
    }

}
