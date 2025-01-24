//// Import packages
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Microsoft.SemanticKernel;
//using Microsoft.SemanticKernel.ChatCompletion;
//using Microsoft.SemanticKernel.Connectors.OpenAI;
//using OpenTelemetry;
//using OpenTelemetry.Resources;
//using OpenTelemetry.Trace;
//using OpenTelemetry.Metrics;
//using Azure.Monitor.OpenTelemetry.Exporter;
//using Microsoft.Extensions.Configuration;
//using System.Configuration;




//// Create a new tracer provider builder and add an Azure Monitor trace exporter to the tracer provider builder.
//// It is important to keep the TracerProvider instance active throughout the process lifetime.
//// See https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/trace#tracerprovider-management
//var tracerProvider = Sdk.CreateTracerProviderBuilder()
//    .AddAzureMonitorTraceExporter();

//// Add an Azure Monitor metric exporter to the metrics provider builder.
//// It is important to keep the MetricsProvider instance active throughout the process lifetime.
//// See https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/metrics#meterprovider-management
//var metricsProvider = Sdk.CreateMeterProviderBuilder()
//    .AddAzureMonitorMetricExporter();

////read configuration value form app.config
//var _appsetting = System.Configuration.ConfigurationManager.AppSettings;

//// Create a new logger factory.
//// It is important to keep the LoggerFactory instance active throughout the process lifetime.
//// See https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/docs/logs#logger-management
//var loggerFactory = LoggerFactory.Create(builder =>
//{
//    builder.AddOpenTelemetry(logging =>
//    {
//        logging.AddAzureMonitorLogExporter(options =>
//        {
//            options.ConnectionString = _appsetting[0];
//        });
//    });
//});



//// Populate values from your OpenAI deployment
//var modelId = "gpt-4o";
//var endpoint = "https://ai-hub-demo-basemodel.openai.azure.com/";
//var apiKey = _appsetting[1];

//// Create a kernel with Azure OpenAI chat completion
//var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);

//// Add enterprise components
//builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

//// Build the kernel
//Kernel kernel = builder.Build();
//var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

//// Add a plugin (the LightsPlugin class is defined below)
//kernel.Plugins.AddFromType<ComputerVisionPlugin>("ComputerVision");
//kernel.Plugins.AddFromType<AIReportPlugin>("AIReport");

//var arguments = new KernelArguments();
//arguments.Add("initialInput", "Provide an image URL for analysis.");

//// Enable planning
//OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
//{
//    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
//};
//// Create a history store the conversation
//var history = new ChatHistory();

//// Initiate a back-and-forth chat
//string? userInput;
//do
//{
//    // Collect user input
//    Console.Write("User > ");
//    userInput = Console.ReadLine();

//    // Add user input
//    history.AddUserMessage(userInput);

//    //add data only if key does not exit
//    if (!kernel.Data.ContainsKey("CustomSetting"))
//        kernel.Data.Add("CustomSetting", "This is a custom setting");
    
//    // Get the response from the AI
//    var result = await chatCompletionService.GetChatMessageContentAsync(
//        history,
//        executionSettings: openAIPromptExecutionSettings,
//        kernel: kernel
//        );


//    // Print the results
//    Console.WriteLine("Assistant > " + result);

//    // Add the message from the agent to the chat history
//    //history.AddMessage(result.Role, result.Content ?? string.Empty);
//} while (userInput is not null);

////public static class GlobalArgs
////{
////   public static dynamic _args = string.Empty;
////}