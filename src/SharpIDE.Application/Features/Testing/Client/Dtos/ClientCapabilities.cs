

using Newtonsoft.Json;

namespace SharpIDE.Application.Features.Testing.Client.Dtos;

public sealed record ClientCapabilities(
    [property: JsonProperty("testing")]
    ClientTestingCapabilities Testing);

public sealed record ClientTestingCapabilities(
    [property: JsonProperty("debuggerProvider")]
    bool DebuggerProvider);
