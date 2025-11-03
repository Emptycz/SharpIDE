

using Newtonsoft.Json;

namespace SharpIDE.Application.Features.Testing.Client.Dtos;

public sealed record TestNodeUpdate
(
    [property: JsonProperty("node")]
    TestNode Node,

    [property: JsonProperty("parent")]
    string ParentUid);

public sealed record TestNode
(
    [property: JsonProperty("uid")]
    string Uid,

    [property: JsonProperty("display-name")]
    string DisplayName,

    [property: JsonProperty("node-type")]
    string NodeType,

    [property: JsonProperty("execution-state")]
    string ExecutionState);
