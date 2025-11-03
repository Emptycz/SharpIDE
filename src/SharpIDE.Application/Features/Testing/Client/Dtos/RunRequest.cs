

using Newtonsoft.Json;

namespace SharpIDE.Application.Features.Testing.Client.Dtos;

public sealed record RunRequest(
    [property:JsonProperty("tests")]
    TestNode[]? TestCases,
    [property:JsonProperty("runId")]
    Guid RunId);
