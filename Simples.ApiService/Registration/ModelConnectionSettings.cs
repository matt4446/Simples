using System.Data.Common;
using Throw;

namespace Simples.ApiService.Registration;

public sealed record ModelConnectionSettings(string ConnectionString, Uri Endpoint, string Model) 
{
    public static ModelConnectionSettings Parse(string connectionString) {

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
        return new ModelConnectionSettings(connectionString, new Uri(endpoint.ToString(), uriKind: UriKind.Absolute), model.ToString());
    }
}