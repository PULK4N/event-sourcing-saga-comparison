using CommunicationModule.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("test-send-to-myself")]
        public async Task<IActionResult> Send(
            [FromServices] IMessageProducer<string, string> messageProducer
        )
        {
            var jobject1 = "test";
            var jobject2 = "test";
            await messageProducer.ProduceAsync("testSendingDataFromSaga", jobject1, jobject2);
            return await Task.FromResult(Ok("sent: " + new { jobject1, jobject2 }));
        }

        [HttpPost("test-send-to-micro-service")]
        public async Task<IActionResult> SendToAll(
            [FromServices] IMessageProducer<string, string> messageProducer
        )
        {
            var healthCheckObject1 = "health check";
            var healthCheckObject2 = "health check";
            await messageProducer.ProduceAsync(
                "notification-events",
                healthCheckObject1,
                healthCheckObject2
            );
            await messageProducer.ProduceAsync(
                "transaction-compensate",
                healthCheckObject1,
                healthCheckObject2
            );
            await messageProducer.ProduceAsync(
                "transaction-complete",
                healthCheckObject1,
                healthCheckObject2
            );
            await messageProducer.ProduceAsync(
                "account-credit-request",
                healthCheckObject1,
                healthCheckObject2
            );
            await messageProducer.ProduceAsync(
                "account-debit-request",
                healthCheckObject1,
                healthCheckObject2
            );
            return await Task.FromResult(
                Ok("sent: " + new { healthCheckObject1, healthCheckObject2 })
            );
        }

        [HttpPost("test-sending-credit-request")]
        public async Task<IActionResult> SendAccountCreditRequest(
            [FromServices] IMessageProducer<string, string> messageProducer
        )
        {
            var jobject1 = "account credit request";
            var jobject2 = "account credit request";
            await messageProducer.ProduceAsync("account-debit-request", jobject1, jobject2);
            return await Task.FromResult(Ok("sent: " + new { jobject1, jobject2 }));
        }
    }
}
