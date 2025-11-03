

using Newtonsoft.Json;

namespace SharpIDE.Application.Features.Testing.Client.Dtos;

public sealed record ServerInfo(
    [property:JsonProperty("name")]
    string Name,

    [property:JsonProperty("version")]
    string Version = "1.0.0");
