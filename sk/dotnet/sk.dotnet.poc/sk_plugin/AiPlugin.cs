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
    private readonly IConfiguration configuration;

    public AiPlugin(IConfiguration _configuration)
    {
        configuration = _configuration;
        // Populate values from your OpenAI deployment
        var modelId = "gpt-4o";
        var endpoint = "https://ai-hub-demo-basemodel.openai.azure.com/";
        var apiKey = _configuration["AppSettings:gpt-apikey"];

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

    public async Task<ChatMessageContent> CallAiOrchestrator(string userInput, string filepath)
    {
        // Add user input
        history.AddUserMessage(userInput);

        // Add data only if key does not exist
        if (!kernel.Data.ContainsKey("CustomSetting"))
            kernel.Data.Add("CustomSetting", "This is a custom setting");

        if (!kernel.Data.ContainsKey("filePath"))
        {
            kernel.Data.Remove("filePath");
            kernel.Data.Add("filePath", filepath);
        }
        if (!kernel.Data.ContainsKey("configuration"))
        {
            kernel.Data.Remove("configuration");
            kernel.Data.Add("configuration", configuration);
        }

        // Get the response from the AI
        var result = await chatCompletionService.GetChatMessageContentAsync(
            history,
            executionSettings: openAIPromptExecutionSettings,
            kernel: kernel
        );
        return result;
    }
}
