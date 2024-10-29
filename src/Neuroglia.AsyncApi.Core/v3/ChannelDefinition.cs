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
/// Represents an object used to define an Async API channel
/// </summary>
[DataContract]
public record ChannelDefinition
    : ReferenceableComponentDefinition
{

    /// <summary>
    /// Gets/sets an optional address of this channel item.
    /// </summary>
    [DataMember(Order = 1, Name = "address"), JsonPropertyOrder(1), JsonPropertyName("address"), YamlMember(Order = 1, Alias = "address")]
    public virtual string? Address { get; set; }

    /// <summary>
    /// Gets/sets an optional description of this channel item. <see href="https://spec.commonmark.org/">CommonMark</see> syntax can be used for rich text representation.
    /// </summary>
    [DataMember(Order = 2, Name = "description"), JsonPropertyOrder(2), JsonPropertyName("description"), YamlMember(Order = 2, Alias = "description")]
    public virtual string? Description { get; set; }

    /// <summary>
    /// Gets/sets a <see cref="Dictionary{TKey, TValue}"/> of the messages associated with the channel.
    /// </summary>
    [DataMember(Order = 3, Name = "messages"), JsonPropertyOrder(3), JsonPropertyName("messages"), YamlMember(Order = 3, Alias = "messages")]
    public virtual EquatableDictionary<string, MessageDefinition>? Messages { get; set; }

    /// <summary>
    /// Gets/sets a <see cref="Dictionary{TKey, TValue}"/> of the parameters included in the channel name. It SHOULD be present only when using channels with expressions (as defined by RFC 6570 section 2.2).
    /// </summary>
    [DataMember(Order = 4, Name = "parameters"), JsonPropertyOrder(4), JsonPropertyName("parameters"), YamlMember(Order = 4, Alias = "parameters")]
    public virtual EquatableDictionary<string, ParameterDefinition>? Parameters { get; set; }

    /// <summary>
    /// Gets/sets an object used to configure the <see cref="ChannelDefinition"/>'s <see cref="IChannelBindingDefinition"/>s
    /// </summary>
    [DataMember(Order = 5, Name = "bindings"), JsonPropertyOrder(5), JsonPropertyName("bindings"), YamlMember(Order = 5, Alias = "bindings")]
    public virtual ChannelBindingDefinitionCollection? Bindings { get; set; }

}
