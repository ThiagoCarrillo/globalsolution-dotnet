using Microsoft.AspNetCore.Mvc;
using Sessions_app.Patterns;
using System.Diagnostics;

namespace Sessions_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly LoggerManager _logger = LoggerManager.GetInstance();

        public IActionResult Index()
        {
            _logger.LogInfo("Acessando a página inicial");
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInfo("Acessando a página de privacidade");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("Ocorreu um erro na aplicação");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    // Classe de modelo para exibição de erros
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

