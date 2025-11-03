

using Newtonsoft.Json;

namespace SharpIDE.Application.Features.Testing.Client.Dtos;

public sealed record InitializeRequest(
    [property:JsonProperty("processId")]
    int ProcessId,

    [property:JsonProperty("clientInfo")]
    ClientInfo ClientInfo,

    [property:JsonProperty("capabilities")]
    ClientCapabilities Capabilities);
