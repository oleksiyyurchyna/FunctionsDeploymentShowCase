using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionsDeploymentShowCase.AzCli
{
    public class FunctionAzCli
    {
        private readonly ILogger<FunctionAzCli> _logger;

        public FunctionAzCli(ILogger<FunctionAzCli> logger)
        {
            _logger = logger;
        }

        [Function(nameof(FunctionAzCli))]
        public async Task Run(
            [ServiceBusTrigger("az-cli-messages", Connection = "AzureWebJobsServiceBus")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
