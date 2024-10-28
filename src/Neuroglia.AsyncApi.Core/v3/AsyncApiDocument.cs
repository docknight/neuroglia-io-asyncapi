﻿// Copyright © 2021-Present Neuroglia SRL. All rights reserved.
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

using YamlDotNet.Core;

namespace Neuroglia.AsyncApi.v3;

/// <summary>
/// Represents an <see href="https://www.asyncapi.com">Async API</see> version 2 document
/// </summary>
[DataContract]
public record AsyncApiDocument
{

    /// <summary>
    /// Gets/sets the the AsyncAPI Specification version being used. It can be used by tooling Specifications and clients to interpret the version. 
    /// </summary>
    /// <remarks>
    /// The structure shall be major.minor.patch, where patch versions must be compatible with the existing major.minor tooling. 
    /// Typically patch versions will be introduced to address errors in the documentation, and tooling should typically be compatible with the corresponding major.minor (1.0.*). 
    /// Patch versions will correspond to patches of this document.
    /// </remarks>
    [Required]
    [DataMember(Order = 1, Name = "asyncapi"), JsonPropertyOrder(1), JsonPropertyName("asyncapi"), YamlMember(Order = 1, Alias = "asyncapi", ScalarStyle = ScalarStyle.SingleQuoted)]
    public virtual string AsyncApi { get; set; } = AsyncApiSpecVersion.V3;

    /// <summary>
    /// Gets/sets the identifier of the application the AsyncAPI document is defining
    /// </summary>
    [DataMember(Order = 2, Name = "id"), JsonPropertyOrder(2), JsonPropertyName("id"), YamlMember(Order = 2, Alias = "id")]
    public virtual string? Id { get; set; }

    /// <summary>
    /// Gets/sets the object that provides metadata about the API. The metadata can be used by the clients if needed. 
    /// </summary>
    [Required]
    [DataMember(Order = 3, Name = "info"), JsonPropertyOrder(3), JsonPropertyName("info"), YamlMember(Order = 3, Alias = "info")]
    public virtual ApiInfo Info { get; set; } = null!;

    /// <summary>
    /// Gets/sets a <see cref="Dictionary{TKey, TValue}"/> containing name/configuration mappings for the application's servers
    /// </summary>
    [DataMember(Order = 4, Name = "servers"), JsonPropertyOrder(4), JsonPropertyName("servers"), YamlMember(Order = 4, Alias = "servers")]
    public virtual EquatableDictionary<string, ServerDefinition>? Servers { get; set; }

    /// <summary>
    /// Gets/sets the default content type to use when encoding/decoding a message's payload.
    /// </summary>
    [DataMember(Order = 5, Name = "defaultContentType"), JsonPropertyOrder(5), JsonPropertyName("defaultContentType"), YamlMember(Order = 5, Alias = "defaultContentType")]
    public virtual string? DefaultContentType { get; set; }

    /// <summary>
    /// Gets/sets a collection containing the available channels and messages for the API.
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Order = 6, Name = "channels"), JsonPropertyOrder(6), JsonPropertyName("channels"), YamlMember(Order = 6, Alias = "channels")]
    public virtual EquatableDictionary<string, ChannelDefinition> Channels { get; set; } = null!;

    /// <summary>
    /// Gets/sets a collection containing the available operations for the API.
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Order = 7, Name = "operations"), JsonPropertyOrder(7), JsonPropertyName("operations"), YamlMember(Order = 7, Alias = "operations")]
    public virtual EquatableDictionary<string, OperationDefinition> Operations { get; set; } = null!;

    /// <summary>
    /// Gets/sets an object used to hold various schemas for the specification.
    /// </summary>
    [DataMember(Order = 8, Name = "components"), JsonPropertyOrder(8), JsonPropertyName("components"), YamlMember(Order = 8, Alias = "components")]
    public virtual ComponentDefinitionCollection? Components { get; set; }

    /// <summary>
    /// Attempts to get the <see cref="OperationDefinition"/> with the specified id
    /// </summary>
    /// <param name="operationId">The id of the <see cref="OperationDefinition"/> to get</param>
    /// <param name="operation">The resulting <see cref="OperationDefinition"/>, if any</param>
    /// <param name="channelName">The name of the <see cref="ChannelDefinition"/> the <see cref="OperationDefinition"/> belongs to, if any</param>
    /// <returns>A boolean indicating whether or not the <see cref="OperationDefinition"/> with the specified id could be found</returns>
    public virtual bool TryGetOperation(string operationId, out OperationDefinition? operation, out string? channelName)
    {
        if (string.IsNullOrWhiteSpace(operationId)) throw new ArgumentNullException(nameof(operationId));
        operation = Operations?.FirstOrDefault(o => o.Value.OperationId == operationId).Value;
        channelName = operation?.Channel?.Reference;
        return true;
    }

    /// <summary>
    /// Attempts to get the <see cref="OperationDefinition"/> with the specified id
    /// </summary>
    /// <param name="operationId">The id of the <see cref="OperationDefinition"/> to get</param>
    /// <param name="operation">The resulting <see cref="OperationDefinition"/>, if any</param>
    /// <returns>A boolean indicating whether or not the <see cref="OperationDefinition"/> with the specified id could be found</returns>
    public virtual bool TryGetOperation(string operationId, out OperationDefinition? operation)
    {
        if (string.IsNullOrWhiteSpace(operationId))
            throw new ArgumentNullException(nameof(operationId));
        return TryGetOperation(operationId, out operation, out _);
    }

    /// <summary>
    /// Determines whether or not the <see cref="ChannelDefinition"/> defines an <see cref="OperationDefinition"/> with the specified id
    /// </summary>
    /// <param name="operationId">The id of the operation to check</param>
    /// <returns>A boolean indicating whether or not the <see cref="ChannelDefinition"/> defines an <see cref="OperationDefinition"/> with the specified id</returns>
    public virtual bool DefinesOperationWithId(string operationId)
    {
        if (string.IsNullOrWhiteSpace(operationId)) throw new ArgumentNullException(nameof(operationId));
        return Operations != null && Operations.Any(o => o.Value.OperationId == operationId);
    }

    public virtual List<MessageDefinition> DereferenceMessageDefinitionsForOperation(OperationDefinition operation)
    {
        if (operation == null)
        {
            throw new ArgumentNullException(nameof(operation));
        }

        var messages = new List<MessageDefinition>();
        var channel = this.DereferenceChannelDefinitionForOperation(operation);
        if (channel == null)
        {
            throw new Exception($"Channel not found for operation {operation.OperationId}.");
        }

        if (operation.Messages == null || channel.Messages == null)
        {
            return messages;
        }

        foreach (var message in operation.Messages)
        {
            if (message.Reference != null)
            {
                if (channel.Messages.TryGetValue(message.Reference.Substring(message.Reference.LastIndexOf("/") + 1), out MessageDefinition? referencedMessage))
                {
                    messages.Add(referencedMessage);
                }
            }
        }

        return messages;
    }

    public virtual ChannelDefinition? DereferenceChannelDefinitionForOperation(OperationDefinition operation)
    {
        if (operation == null)
        {
            throw new ArgumentNullException(nameof(operation));
        }

        if (operation.Channel?.Reference == null)
        {
            return null;
        }

        if (this.Channels.TryGetValue(operation.Channel.Reference.Substring("#channels/".Length + 1), out ChannelDefinition? channel))
        {
            return channel;
        }

        return null;
    }

    /// <inheritdoc/>
    public override string? ToString() => Info == null || string.IsNullOrWhiteSpace(Info.Title) ? Id : Info?.Title;

}
