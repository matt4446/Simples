using Microsoft.SemanticKernel;
using Simples.ApiService.Services.HomeAutomation;
using Simples.ApiService.Tools;
using Simples.ApiService.Agents;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

namespace Simples.ApiService.Registration;
public static class AiRegiistrationExtensions
{
#pragma warning disable SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    public static void AddLocalChatClients(this WebApplicationBuilder web) 
    {
        web.Services.AddScoped<GetFromHomeAutomationSerivce>();
        web.Services.AddScoped<HomeAutomationWebSocket>();
        web.Services.AddScoped<UpdateHomeAutomationService>();

        var modelConnectionString = web.Configuration.GetConnectionString("ollama-llama3-3");
        var modelSettings = AspireModelConnectionSettings.Parse(modelConnectionString!);

        web.Services.AddScoped<HomeAssistandPlugin>();
        web.Services.AddTransient<HomeAssistantAgent>();
        web.Services.AddKeyedTransient<Kernel>("HomeAssistantKernal", (sp, key) =>
        {
            //KernelPluginCollection pluginCollection = [];
            //pluginCollection.AddFromObject(sp.GetRequiredService<HomeAssistandPlugin>());

            IKernelBuilder builder = Kernel.CreateBuilder();
            builder.Services.AddTransient<HomeAssistandPlugin>();
            builder.Services.AddOllamaChatCompletion(modelSettings.Model, modelSettings.Endpoint);
            builder.Plugins.AddFromObject(sp.GetRequiredService<HomeAssistandPlugin>());
            var kernel = builder.Build();

            return kernel;
            //return new Kernel(sp, pluginCollection);
        });

        //builder.Services.AddTransient<IChatClient>(serviceProvider => {
        //    var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        //    var chatClient = new OllamaChatClient(phiSettings.Endpoint, phiSettings.Model);

        //    ChatOptions chatOptions = new()
        //    {
        //        Tools = [AIFunctionFactory.Create(GetWeather)]
        //    };

        //    return chatClient;
        //});


        
    }

#pragma warning restore SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

}

