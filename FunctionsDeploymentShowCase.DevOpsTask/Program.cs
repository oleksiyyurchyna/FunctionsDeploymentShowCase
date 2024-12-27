using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHostBuilder builder = new HostBuilder();

builder.ConfigureHostConfiguration(x => x.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true,
		reloadOnChange: true));

var hostBuilder = builder
	.ConfigureFunctionsWebApplication()
	.ConfigureServices((context, services) =>
	{
		services.AddApplicationInsightsTelemetryWorkerService();
		services.ConfigureFunctionsApplicationInsights();

		services.AddSingleton(serviceProvider =>
		{
			var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsServiceBus");
			var client = new ServiceBusClient(connectionString);
			return client.CreateSender("devops-task-messages");
		});
	});

var host = hostBuilder.Build();

host.Run();
