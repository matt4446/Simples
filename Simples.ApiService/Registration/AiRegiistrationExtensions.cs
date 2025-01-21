using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel;
using OllamaSharp.Models;
using Simples.ApiService.Services.HomeAutomation;
using Microsoft.Extensions.AI;

namespace Simples.ApiService.Registration;
public static class AiRegiistrationExtensions
{
#pragma warning disable SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    public static void AddLocalChatClients(this WebApplicationBuilder builder) 
    {
        builder.Services.AddScoped<GetFromHomeAutomationSerivce>();
        builder.Services.AddScoped<HomeAutomationWebSocket>();
        builder.Services.AddScoped<UpdateHomeAutomationService>();

        // doesnt support tools 
        //builder.AddOllamaSharpChatClient("ollama-phi3-5");
        //builder.AddOllamaSharpChatClient("phi4");

        //builder.AddOllamaSharpChatClient("ollama-llama3-3");

        // migrate to microsoft unified ...? 
        builder.AddKeyedOllamaSharpChatClient("phi4");
        builder.Services.AddChatClient(sp => sp.GetRequiredKeyedService<IChatClient>("phi4"));

        //builder.AddKeyedOllamaSharpChatClient("codellama");

        //var settings = new OllamaPromptExecutionSettings { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
        //Endpoint=http://localhost:55206;Model=hf.co/matteogeniaccio/phi-4
        //var modelId = "phi4";
        //var endpoint = ";

        //builder.Services.AddOllamaChatCompletion(modelId: "phi4", endpoint: "http://localhost:55206", serviceId: null);

        //var kernalBuilder = Kernel.CreateBuilder();
        //kernalBuilder.AddInMemoryVectorStore();
        //kernalBuilder.AddOllamaChatCompletion(modelId: modelId, endpoint: endpoint, serviceId: null);


        ////builder.Services.AddOllamaChatCompletion(modelId, endpoint, null);

        //builder.AddOllamaSharpChatClient("phi4");
    }

#pragma warning restore SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

}
