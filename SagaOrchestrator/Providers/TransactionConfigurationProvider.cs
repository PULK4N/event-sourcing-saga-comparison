using Microsoft.Extensions.Configuration;

namespace SagaOrchestrator
{
  public class TransactionConfigurationProvider
  {
        private readonly IConfiguration configuration;

        public TransactionConfigurationProvider(IConfiguration configuration)
        {
            configuration = configuration;
        }
  }
}
