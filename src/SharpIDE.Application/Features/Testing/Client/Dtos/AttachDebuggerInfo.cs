using Newtonsoft.Json;

namespace SharpIDE.Application.Features.Testing.Client.Dtos;

public sealed record AttachDebuggerInfo(
    [property:JsonProperty("processId")]
    int ProcessId);
