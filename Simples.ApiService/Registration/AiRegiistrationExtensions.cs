using Microsoft.SemanticKernel;
using Simples.ApiService.Services.HomeAutomation;
using Simples.ApiService.Tools;
using Simples.ApiService.Agents;

namespace Simples.ApiService.Registration;
public static class AiRegiistrationExtensions
{
#pragma warning disable SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    public static void AddLocalChatClients(this WebApplicationBuilder builder) 
    {
        builder.Services.AddScoped<GetFromHomeAutomationSerivce>();
        builder.Services.AddScoped<HomeAutomationWebSocket>();
        builder.Services.AddScoped<UpdateHomeAutomationService>();

        var phiCconnectionString = builder.Configuration.GetConnectionString("phi4");
        var phiSettings = AspireModelConnectionSettings.Parse(phiCconnectionString!);

        builder.Services.AddScoped<HomeAssistandPlugin>();
        builder.Services.AddOllamaChatCompletion(phiSettings.Model, phiSettings.Endpoint);
        builder.Services.AddKeyedTransient("HomeAssistantKernal", (sp, key) => {
            KernelPluginCollection pluginCollection = [];
            pluginCollection.AddFromObject(sp.GetRequiredService<HomeAssistandPlugin>());
            
            return new Kernel(sp, pluginCollection);
        });


        builder.Services.AddTransient<HomeAssistantClient>();
    }

#pragma warning restore SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

}

