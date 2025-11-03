

using Newtonsoft.Json;

namespace SharpIDE.Application.Features.Testing.Client.Dtos;

public sealed record DiscoveryRequest(
    [property:JsonProperty("runId")]
    Guid RunId);
