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

namespace Neuroglia.AsyncApi.FluentBuilders;

/// <summary>
/// Defines the fundamentals of a service used to build <see cref="OperationDefinition"/>s
/// </summary>
public interface IOperationDefinitionBuilder
    : IOperationTraitDefinitionBuilder<IOperationDefinitionBuilder, OperationDefinition>
{

    /// <summary>
    /// Configures the <see cref="OperationDefinition"/> to build to use the specified <see cref="OperationTraitDefinition"/>
    /// </summary>
    /// <param name="setup">An <see cref="Action{T}"/> used to setup the <see cref="OperationTraitDefinition"/> to use</param>
    /// <returns>The configured <see cref="IOperationDefinitionBuilder"/></returns>
    IOperationDefinitionBuilder WithTrait(Action<IOperationTraitBuilder> setup);

    /// <summary>
    /// Configures the <see cref="OperationDefinition"/> to build to use the specified <see cref="MessageDefinition"/> reference.
    /// </summary>
    /// <param name="messageId">An identifier for the <see cref="MessageDefinition"/> to use as a reference.</param>
    /// <returns>The configured <see cref="IOperationDefinitionBuilder"/></returns>
    IOperationDefinitionBuilder WithReferenceToMessageDefinition(string messageId);

    /// <summary>
    /// Configures the <see cref="OperationDefinition"/> to build to use the specified <see cref="ChannelDefinition"/> reference.
    /// </summary>
    /// <param name="channelId">An identifier for the <see cref="ChannelDefinition"/> to use as a reference.</param>
    /// <returns>The configured <see cref="IOperationDefinitionBuilder"/></returns>
    IOperationDefinitionBuilder WithReferenceToChannelDefinition(string channelId);

    /// <summary>
    /// Sets the <see cref="OperationDefinition"/>'s <see cref="ActionType"/>
    /// </summary>
    /// <param name="type">The type of the operation.</param>
    /// <returns>The configured <see cref="IOperationDefinitionBuilder"/></returns>
    IOperationDefinitionBuilder WithActionType(ActionType type);

}
