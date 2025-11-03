

using Newtonsoft.Json;

namespace SharpIDE.Application.Features.Testing.Client.Dtos;

public record TelemetryPayload
(
    [property: JsonProperty(nameof(TelemetryPayload.EventName))]
    string EventName,

    [property: JsonProperty("metrics")]
    IDictionary<string, string> Metrics);
