using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithLifetime(ContainerLifetime.Persistent);

//https://github.com/dotnet/docs-aspire/blob/main/docs/community-toolkit/ollama.md
var ollama = builder.AddOllama("ollama")
    .WithDataVolume()
    .WithContainerRuntimeArgs("--gpus=all")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithOpenWebUI()
    .PublishAsContainer();

var llama = ollama.AddModel("llama3.2");
var deepseek = ollama.AddModel("deepseek-r1");
//var codellama = ollama.AddModel("codellama");
// openChat = ollama.AddModel("openchat");
// embed 
// https://dataloop.ai/library/model/nomic-ai_nomic-embed-text-v15/#:~:text=It%E2%80%99s%20simple%3A%20just%20add%20a%20task%20instruction%20prefix,with%20questions%2C%20you%20can%20use%20the%20search_query%20prefix.
//var nomic = ollama.AddModel("nomic-embed-text");
// doesnt support tools :-/
//var phi4 = ollama.AddHuggingFaceModel("phi4", "matteogeniaccio/phi-4");
//var phi35 = ollama.AddModel("phi3.5");

var homeAssistant = builder.AddContainer("homeassistant", "homeassistant/home-assistant")
    .WithVolume("config", "/config")
    .WithHttpEndpoint(targetPort: 8123)
    .WithLifetime(ContainerLifetime.Persistent);

var homeAssistantHttp = homeAssistant.GetEndpoint("http");
var homeAssistantHttps = homeAssistant.GetEndpoint("https");

var apiService = builder
    .AddProject<Projects.Simples_ApiService>("apiservice")
    .WithReference(homeAssistantHttp)
    .WithReference(homeAssistantHttps)
    .WithReference(llama)
    .WithReference(deepseek)
    ;
    //.WithReference(codellama)
    //.WithReference(openChat)
    //.WithReference(phi4);

builder.AddNpmApp("svelete", "../Simples.Svelete")
    //.WithReference(apiService)
    .WaitFor(apiService)
    .WithEnvironment("VITE_ApiService", apiService.GetEndpoint("https"))
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "5173", targetPort: 5173)
    .WithExternalHttpEndpoints()
    //.WithEnvironment("API_SERVICE_URL", () => apiService.Resource.GetEndpoints().FirstOrDefault().Url)
    .PublishAsDockerFile();

builder.Build().Run();
