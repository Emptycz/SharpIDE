

using Newtonsoft.Json;

namespace SharpIDE.Application.Features.Testing.Client.Dtos;

public sealed record ServerCapabilities(
    [property: JsonProperty("testing")]
    ServerTestingCapabilities Testing);

public sealed record ServerTestingCapabilities(
    [property: JsonProperty("supportsDiscovery")]
    bool SupportsDiscovery,
    [property: JsonProperty("experimental_multiRequestSupport")]
    bool MultiRequestSupport,
    [property: JsonProperty("vsTestProvider")]
    bool VSTestProvider);
