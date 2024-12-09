var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithLifetime(ContainerLifetime.Persistent);

//https://github.com/dotnet/docs-aspire/blob/main/docs/community-toolkit/ollama.md
var ollama = builder.AddOllama("ollama")
    .WithContainerRuntimeArgs("--gpus=all")
    .WithLifetime(ContainerLifetime.Persistent);

var phi35 = ollama.AddModel("phi3.5");


builder.AddContainer("homeassistant", "homeassistant/home-assistant")
    .WithVolume("config", "/etc/homeassistant")
    .WithLifetime(ContainerLifetime.Persistent);

var apiService = builder
    .AddProject<Projects.Simples_ApiService>("apiservice");

builder.AddProject<Projects.Simples_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WithReference(phi35)
    .WaitFor(apiService);

builder.Build().Run();
