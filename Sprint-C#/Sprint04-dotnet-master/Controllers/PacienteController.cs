using Microsoft.AspNetCore.Mvc;
using Sessions_app.Services;
using Sessions_app.Models;
using Sessions_app.Patterns;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using Sessions_app.Service;


public class PacienteController : Controller
{

    private readonly PacienteService _service;
    private readonly RabbitMqService _rabbitMqService;
    private readonly LoggerManager _logger = LoggerManager.GetInstance();
    private readonly MedicoService _medicoService;

    public PacienteController(PacienteService service, RabbitMqService rabbitMqService, MedicoService medicoService)
    {
        _service = service;
        _rabbitMqService = rabbitMqService;
        _medicoService = medicoService;
    }

    // GET: Paciente
    public async Task<IActionResult> Index()
    {
        _logger.LogInfo("Controller MVC: Listando todos os pacientes");
        var pacientes = await _service.GetAllPacientesAsync();
        return View(pacientes);
    }

    // GET: Paciente/Details/5
    public async Task<IActionResult> Details(int id)
    {
        _logger.LogInfo($"Controller MVC: Exibindo detalhes do paciente ID: {id}");
        var paciente = await _service.GetPacienteByIdAsync(id);
        if (paciente == null)
        {
            _logger.LogWarning($"Paciente com ID {id} não encontrado");
            return NotFound();
        }
        return View(paciente);
    }

    // GET: Paciente/Create
    public IActionResult Create()
    {
        _logger.LogInfo("Controller MVC: Exibindo formulário para criar paciente");
        return View();
    }

    // POST: Paciente/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Paciente paciente)
    {
        if (ModelState.IsValid)
        {
            _logger.LogInfo($"Controller MVC: Criando paciente {paciente.Nome}");
            await _service.CreatePacienteAsync(paciente);
            var medicos = await _medicoService.GetAllMedicosAsync();
            var emailsMedicos = medicos.Select(m => m.Email).ToList();
            _rabbitMqService.PublishNewPatient(paciente, emailsMedicos);
            return RedirectToAction(nameof(Index));
        }
        return View(paciente);
    }

    // GET: Paciente/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        _logger.LogInfo($"Controller MVC: Exibindo formulário para editar paciente ID: {id}");
        var paciente = await _service.GetPacienteByIdAsync(id);
        if (paciente == null)
        {
            _logger.LogWarning($"Paciente com ID {id} não encontrado");
            return NotFound();
        }
        return View(paciente);
    }

    // POST: Paciente/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Paciente paciente)
    {
        if (id != paciente.IdPaciente)
        {
            _logger.LogWarning($"ID {id} não corresponde ao ID do paciente {paciente.IdPaciente}");
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _logger.LogInfo($"Controller MVC: Atualizando paciente ID: {id}");
            await _service.UpdatePacienteAsync(paciente);
            return RedirectToAction(nameof(Index));
        }
        return View(paciente);
    }

    // GET: Paciente/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInfo($"Controller MVC: Exibindo confirmação para excluir paciente ID: {id}");
        var paciente = await _service.GetPacienteByIdAsync(id);
        if (paciente == null)
        {
            _logger.LogWarning($"Paciente com ID {id} não encontrado");
            return NotFound();
        }
        return View(paciente);
    }

    // POST: Paciente/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        _logger.LogInfo($"Controller MVC: Excluindo paciente ID: {id}");
        await _service.DeletePacienteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult ErroPacienteNaoEncontrado(int id)
    {
        ViewData["PacienteId"] = id;
        return View();
    }

    [HttpGet]
    public IActionResult CuidadosDentais()
    {
        return View();
    }
}