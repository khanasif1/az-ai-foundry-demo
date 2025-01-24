using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Extensions.Configuration;
using System.Configuration;

public class AiPlugin
{
    private readonly TracerProvider tracerProvider;
    private readonly MeterProvider metricsProvider;
    private readonly ILoggerFactory loggerFactory;
    private readonly Kernel kernel;
    private readonly IChatCompletionService chatCompletionService;
    private readonly ChatHistory history;
    private readonly OpenAIPromptExecutionSettings openAIPromptExecutionSettings;

    public AiPlugin()
    {
        //// Create a new tracer provider builder and add an Azure Monitor trace exporter to the tracer provider builder.
        //// It is important to keep the TracerProvider instance active throughout the process lifetime.
        //// See https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace#tracerprovider-management
        //tracerProvider = Sdk.CreateTracerProviderBuilder()
        //    .AddAzureMonitorTraceExporter()
        //    .Build();

        //// Add an Azure Monitor metric exporter to the metrics provider builder.
        //// It is important to keep the MetricsProvider instance active throughout the process lifetime.
        //// See https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/metrics#meterprovider-management
        //metricsProvider = Sdk.CreateMeterProviderBuilder()
        //    .AddAzureMonitorMetricExporter()
        //    .Build();

        //// Read configuration value from app.config
        //var _appsetting = System.Configuration.ConfigurationManager.AppSettings;

        //// Create a new logger factory.
        //// It is important to keep the LoggerFactory instance active throughout the process lifetime.
        //// See https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/logs#logger-management
        //loggerFactory = LoggerFactory.Create(builder =>
        //{
        //    builder.AddOpenTelemetry(logging =>
        //    {
        //        logging.AddAzureMonitorLogExporter(options =>
        //        {
        //            options.ConnectionString = _appsetting[0];
        //        });
        //    });
        //});

        // Populate values from your OpenAI deployment
        var modelId = "gpt-4o";
        var endpoint = "https://ai-hub-demo-basemodel.openai.azure.com/";
        var apiKey = "EwAfYEWgZWfzJwZjWgAcRnirKcEqZgJ6XjEUGeROrklnVMum3HiBJQQJ99ALACYeBjFXJ3w3AAAAACOGXzsT";//_appsetting[1];

        // Create a kernel with Azure OpenAI chat completion
        var kernelBuilder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);

        // Add enterprise components
        kernelBuilder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

        // Build the kernel
        kernel = kernelBuilder.Build();
        chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        // Add plugins
        kernel.Plugins.AddFromType<ComputerVisionPlugin>("ComputerVision");
        kernel.Plugins.AddFromType<AIReportPlugin>("AIReport");

        // Enable planning
        openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        // Create a history to store the conversation
        history = new ChatHistory();
    }

    public async Task CallAiOrchestrator(string userInput)
    {
        // Add user input
        history.AddUserMessage(userInput);

        // Add data only if key does not exist
        if (!kernel.Data.ContainsKey("CustomSetting"))
            kernel.Data.Add("CustomSetting", "This is a custom setting");

        // Get the response from the AI
        var result = await chatCompletionService.GetChatMessageContentAsync(
            history,
            executionSettings: openAIPromptExecutionSettings,
            kernel: kernel
        );
    }
}
