using System.Data.Common;
using Throw;

namespace Simples.ApiService.Registration;

public sealed record AspireModelConnectionSettings(string ConnectionString, Uri Endpoint, string Model) 
{
    public static AspireModelConnectionSettings Parse(string connectionString) {

        var connectionBuilder = new DbConnectionStringBuilder
        {
            ConnectionString = connectionString
        };

        var hasEndpoint = connectionBuilder.TryGetValue("Endpoint", out var endpoint);
        var hadModel = connectionBuilder.TryGetValue("Model", out var model);

        if (!hasEndpoint) {
            endpoint.ThrowIfNull();
        }
        if (!hadModel) {
            model.ThrowIfNull();
        }
        return new AspireModelConnectionSettings(connectionString, new Uri(endpoint.ToString(), uriKind: UriKind.Absolute), model.ToString());
    }
}