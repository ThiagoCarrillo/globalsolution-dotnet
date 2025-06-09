using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Sessions_app.Models;
using Sessions_app.Service;
using Sessions_app.Services;

namespace Sessions_app.Controllers
{
    [Route("PrevisaoPaciente")]
    public class PrevisaoPacienteController : Controller
    {
        private readonly PacienteMLService _mlService;

        public PrevisaoPacienteController(PacienteMLService mlService)
        {
            _mlService = mlService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Resultado")]
        public IActionResult Resultado(PacienteData dados)
        {
            var predicao = _mlService.Prever(dados);
            return View(predicao);
        }
    }
}
