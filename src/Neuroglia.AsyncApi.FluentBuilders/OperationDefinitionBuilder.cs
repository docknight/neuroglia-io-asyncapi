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
/// Represents the base class for all <see cref="IOperationDefinitionBuilder"/> implementations
/// </summary>
/// <remarks>
/// Initializes a new <see cref="OperationDefinitionBuilder"/>
/// </remarks>
/// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
/// <param name="validators">An <see cref="IEnumerable{T}"/> containing the services used to validate <see cref="OperationTraitDefinition"/>s</param>
public class OperationDefinitionBuilder(IServiceProvider serviceProvider, IEnumerable<IValidator<OperationTraitDefinition>> validators)
    : OperationTraitDefinitionBuilder<IOperationDefinitionBuilder, OperationDefinition>(serviceProvider, validators), IOperationDefinitionBuilder
{

    /// <inheritdoc/>
    public virtual IOperationDefinitionBuilder WithReferenceToMessageDefinition(string messageId)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(messageId);
        if (string.IsNullOrEmpty(this.Trait.Channel?.Reference))
        {
            throw new InvalidOperationException($"The operation's channel reference must be set before adding message references.");
        }

        this.Trait.Messages ??= new();
        this.Trait.Messages.Add(new ReferenceableComponentDefinition { Reference = $"{this.Trait.Channel?.Reference}/messages/{messageId}"});
        return this;
    }

    /// <inheritdoc/>
    public virtual IOperationDefinitionBuilder WithReferenceToChannelDefinition(string channelId)
    {         
        ArgumentNullException.ThrowIfNullOrEmpty(channelId);
        this.Trait.Channel = new ReferenceableComponentDefinition { Reference = $"#/channels/{channelId}" };
        return this;
    }

    /// <inheritdoc/>
    public virtual IOperationDefinitionBuilder WithTrait(Action<IOperationTraitBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);

        this.Trait.Traits ??= [];
        var builder = ActivatorUtilities.CreateInstance<OperationTraitBuilder>(this.ServiceProvider);
        setup(builder);
        this.Trait.Traits.Add(builder.Build());

        return this;
    }

    /// <inheritdoc/>
    public virtual IOperationDefinitionBuilder WithActionType(ActionType type)
    {
        this.Trait.Action = type;
        return this;
    }
}
