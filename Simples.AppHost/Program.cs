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

var llama = ollama.AddModel("llama3.3");
var codellama = ollama.AddModel("codellama");
var openChat = ollama.AddModel("openchat");
// doesnt support tools :-/
//var phi4 = ollama.AddHuggingFaceModel("phi4", "matteogeniaccio/phi-4");
//var phi35 = ollama.AddModel("phi3.5");

var homeAssistant = builder.AddContainer("homeassistant", "homeassistant/home-assistant")
    //.WithArgs("--net=host")
    .WithVolume("config", "/opt/simples/config")
    .WithVolume("data", "/opt/simples/data" )
    .WithHttpEndpoint(env: "8123", targetPort: 8123)
    .WithExternalHttpEndpoints()
    .WithLifetime(ContainerLifetime.Persistent);

var homeAssistantHttp = homeAssistant.GetEndpoint("http");

var apiService = builder
    .AddProject<Projects.Simples_ApiService>("apiservice")
    .WithReference(homeAssistantHttp)
    .WithReference(llama)
    .WithReference(codellama)
    .WithReference(openChat);
    //.WithReference(phi4);

builder.AddNpmApp("svelete", "../Simples.Svelete")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "5173", targetPort: 5173)
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

//builder.AddProject<Projects.Simples_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithReference(cache)
//    .WaitFor(cache)
//    .WithReference(apiService)
//    .WithReference(phi35)
//    .WaitFor(apiService);

builder.Build().Run();
