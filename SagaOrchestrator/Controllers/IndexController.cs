using Microsoft.AspNetCore.Mvc;

namespace SagaOrchestrator.Controllers
{
    [Route("[controller]")]
    public class IndexController : Controller
    {
        private readonly ILogger<IndexController> _logger;

        public IndexController(ILogger<IndexController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("This was a success");
        }
    }
}
