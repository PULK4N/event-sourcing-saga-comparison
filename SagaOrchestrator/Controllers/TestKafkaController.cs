using CommunicationModule.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SagaOrchestrator.Models;

namespace SagaOrchestrator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestKafkaController : ControllerBase
    {
        private readonly ILogger<TestKafkaController> _logger;

        public TestKafkaController(ILogger<TestKafkaController> logger)
        {
            _logger = logger;
        }

        [HttpPost("test-send-to-micro-service")]
        public async Task<IActionResult> Send(
            [FromServices] IMessageProducer<string, string> messageProducer
        )
        {
            var jobject1 = "test";
            var jobject2 = "test";
            await messageProducer.ProduceAsync(
                "testSendingDataFromSaga",
                jobject1,
                jobject2
            );
            return await Task.FromResult(Ok("sent: " + new { jobject1, jobject2 }));
        }
    }
}
