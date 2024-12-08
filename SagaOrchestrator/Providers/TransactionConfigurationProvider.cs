using Microsoft.Extensions.Configuration;

namespace SagaOrchestrator
{
    public class TransactionConfigurationProvider
    {
        private readonly IConfiguration _configuration;

        public TransactionConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}

// Implementirati da kafka svima moze da salje poruke
