using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionsDeploymentShowCase.DevOpsTask
{
    public class FunctionDevOpsTask
    {
        private readonly ILogger<FunctionDevOpsTask> _logger;

        public FunctionDevOpsTask(ILogger<FunctionDevOpsTask> logger)
        {
            _logger = logger;
        }

        [Function(nameof(FunctionDevOpsTask))]
        public async Task Run(
            [ServiceBusTrigger("devops-task-messages", Connection = "AzureWebJobsServiceBus")]
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
