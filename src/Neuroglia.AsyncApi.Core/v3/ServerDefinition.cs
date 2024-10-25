// Copyright © 2021-Present Neuroglia SRL. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License"),
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Neuroglia.AsyncApi.v3.Bindings;

namespace Neuroglia.AsyncApi.v3;

/// <summary>
/// Represents an object used to describe a message broker, a server or any other kind of computer program capable of sending and/or receiving data.
/// </summary>
/// <remarks>
/// This object is used to capture details such as URIs, protocols and security configuration. 
/// Variable substitution can be used so that some details, for example usernames and passwords, can be injected by code generation tools.
/// </remarks>
[DataContract]
public record ServerDefinition
{

    /// <summary>
    /// Gets/sets the host name.
    /// </summary>
    [Required]
    [DataMember(Order = 1, Name = "host"), JsonPropertyOrder(1), JsonPropertyName("host"), YamlMember(Order = 1, Alias = "host")]
    public virtual string Host { get; set; } = null!;

    /// <summary>
    /// Gets/sets the path on the host, e.g. /production.
    /// </summary>
    [Required]
    [DataMember(Order = 2, Name = "pathname"), JsonPropertyOrder(2), JsonPropertyName("pathname"), YamlMember(Order = 2, Alias = "pathname")]
    public virtual string Pathname { get; set; } = null!;

    /// <summary>
    /// Gets/sets the protocol this URL supports for connection. 
    /// Supported protocol include, but are not limited to: amqp, amqps, http, https, ibmmq, jms, kafka, kafka-secure, mqtt, secure-mqtt, stomp, stomps, ws, wss, mercure.
    /// </summary>
    [Required]
    [DataMember(Order = 3, Name = "protocol"), JsonPropertyOrder(3), JsonPropertyName("protocol"), YamlMember(Order = 3, Alias = "protocol")]
    public virtual string Protocol { get; set; } = null!;

    /// <summary>
    /// Gets/sets the version of the protocol used for connection. For instance: AMQP 0.9.1, HTTP 2.0, Kafka 1.0.0, etc.
    /// </summary>
    [Required]
    [DataMember(Order = 4, Name = "protocolVersion"), JsonPropertyOrder(4), JsonPropertyName("protocolVersion"), YamlMember(Order = 4, Alias = "protocolVersion")]
    public virtual string ProtocolVersion { get; set; } = null!;

    /// <summary>
    /// Gets/sets an optional string describing the host designated by the URL. <see href="https://spec.commonmark.org/">CommonMark</see> syntax MAY be used for rich text representation.
    /// </summary>
    [DataMember(Order = 5, Name = "description"), JsonPropertyOrder(5), JsonPropertyName("description"), YamlMember(Order = 5, Alias = "description")]
    public virtual string? Description { get; set; }

    /// <summary>
    /// Gets/sets an optional string describing the host designated by the URL. <see href="https://spec.commonmark.org/">CommonMark</see> syntax MAY be used for rich text representation.
    /// </summary>
    [DataMember(Order = 6, Name = "variables"), JsonPropertyOrder(6), JsonPropertyName("variables"), YamlMember(Order = 6, Alias = "variables")]
    public virtual EquatableDictionary<string, VariableDefinition>? Variables { get; set; }

    /// <summary>
    /// Gets/sets an <see cref="IList{T}"/> of values that represent alternative security requirement objects that can be used. 
    /// Only one of the security requirement objects need to be satisfied to authorize a connection or operation. 
    /// </summary>
    [DataMember(Order = 7, Name = "security"), JsonPropertyOrder(7), JsonPropertyName("security"), YamlMember(Order = 7, Alias = "security")]
    public virtual EquatableDictionary<string, object>? Security { get; set; }

    /// <summary>
    /// Gets/sets an object used to configure the <see cref="ServerDefinition"/>'s <see cref="IServerBindingDefinition"/>s
    /// </summary>
    [DataMember(Order = 8, Name = "bindings"), JsonPropertyOrder(8), JsonPropertyName("bindings"), YamlMember(Order = 8, Alias = "bindings")]
    public virtual ServerBindingDefinitionCollection? Bindings { get; set; }

    /// <summary>
    /// Interpolates the defined server's url variables
    /// </summary>
    /// <returns>The interpolated server url</returns>
    public virtual Uri InterpolateUrlVariables()
    {
        var url = new Uri($"{Protocol}://{Host}{Pathname}", UriKind.RelativeOrAbsolute);
        if (Variables == null || Variables.Count == 0) return url;
        var rawUrl = url.ToString();
        foreach (var variable in Variables)
        {
            var value = variable.Value.Default;
            if (string.IsNullOrWhiteSpace(value)) value = variable.Value.Enum?.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(value)) continue;
            rawUrl = rawUrl.Replace($"{{{variable.Key}}}", value);
        }
        return new Uri(rawUrl, UriKind.RelativeOrAbsolute);
    }

    /// <inheritdoc/>
    public override string ToString() => $"{Protocol}://{Host}{Pathname}";

}
