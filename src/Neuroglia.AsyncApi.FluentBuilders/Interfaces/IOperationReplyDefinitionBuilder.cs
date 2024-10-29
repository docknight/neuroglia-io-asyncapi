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

namespace Neuroglia.AsyncApi.FluentBuilders.Interfaces;

/// <summary>
/// Defines the fundamentals of a service used to build <see cref="OperationReplyDefinition"/>s
/// </summary>
public interface IOperationReplyDefinitionBuilder
{
    /// <summary>
    /// Adds a channel reference to the <see cref="OperationReplyDefinition"/> to build.
    /// </summary>
    /// <param name="channelId">Channel ID.</param>
    /// <returns>The configured <see cref="IOperationReplyDefinitionBuilder"/></returns>
    IOperationReplyDefinitionBuilder WithChannelReference(string channelId);

    /// <summary>
    /// Adds a message reference to the <see cref="OperationReplyDefinition"/> to build.
    /// </summary>
    /// <param name="messageId">Message id.</param>
    /// <param name="channelId">Id of the channel the message is associated with.</param>
    /// <returns>The configured <see cref="IOperationReplyDefinitionBuilder"/></returns>
    IOperationReplyDefinitionBuilder WithMessageReference(string messageId, string channelId);

    /// <summary>
    /// Adds an address reference to the <see cref="OperationReplyDefinition"/> to build.
    /// </summary>
    /// <param name="addressId">Address ID.</param>
    /// <returns>The configured <see cref="IOperationReplyDefinitionBuilder"/></returns>
    IOperationReplyDefinitionBuilder WithAddressReference(string addressId);

    /// <summary>
    /// Adds an address to the <see cref="OperationReplyDefinition"/> to build.
    /// </summary>
    /// <param name="description">An optional description of the address. CommonMark syntax can be used for rich text representation.</param>
    /// <param name="location">A runtime expression that specifies the location of the reply address.</param>
    /// <returns>The configured <see cref="IOperationReplyDefinitionBuilder"/></returns>
    IOperationReplyDefinitionBuilder WithAddress(string? description, string location);

    /// <summary>
    /// Builds a new <see cref="OperationReplyDefinition"/>
    /// </summary>
    /// <returns>A new <see cref="OperationReplyDefinition"/></returns>
    OperationReplyDefinition Build();

}
