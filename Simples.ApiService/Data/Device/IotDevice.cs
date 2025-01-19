using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.InMemory;

namespace Simples.ApiService.Data.Device;

public class IotDevice
{
    [VectorStoreRecordKey]
    public int Key { get; set; }

    [VectorStoreRecordData(IsFilterable = true)]
    public required string Name { get; set; }

    [VectorStoreRecordData(IsFullTextSearchable = true)]
    public required string Description { get; set; }

    [VectorStoreRecordVector(4, DistanceFunction.CosineDistance, IndexKind.Hnsw)]
    public DeviceType DeviceType { get; set; }

    public ReadOnlyMemory<float> Vector { get; set; }

    public string Room { get; set; } = null!;

    [VectorStoreRecordData(IsFilterable = true)]
    public string[] Tags { get; set; } = [];
}

public enum DeviceType
{
    Light = 1
}


public sealed class DeviceStore
{
    private InMemoryVectorStore store = new InMemoryVectorStore();

    public IVectorStoreRecordCollection<int, IotDevice>? DevicesStore { get; private set; }

    public async Task Init()
    {
        this.DevicesStore = store.GetCollection<int, IotDevice>("devices");

        await DevicesStore.CreateCollectionIfNotExistsAsync();


        //IEmbeddingGenerator<string, Embedding<float>>
        //    generator = new OllamaEmbeddingGenerator();
    }

    public Task GetDevices()
    {
        var deviceCollection = store.GetCollection<int, IotDevice>("devices");

        return Task.FromResult(deviceCollection);
    }

}