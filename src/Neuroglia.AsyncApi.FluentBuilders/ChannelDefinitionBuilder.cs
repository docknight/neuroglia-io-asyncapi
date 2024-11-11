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

using Neuroglia.AsyncApi.v3;
using Neuroglia.AsyncApi.v3.Bindings;

namespace Neuroglia.AsyncApi.FluentBuilders;

/// <summary>
/// Represents the default implementation of the <see cref="IChannelDefinitionBuilder"/>
/// </summary>
/// <remarks>
/// Initializes a new <see cref="ChannelDefinitionBuilder"/>
/// </remarks>
/// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
/// <param name="validators">The services used to validate <see cref="ChannelDefinition"/>s</param>
public class ChannelDefinitionBuilder(IServiceProvider serviceProvider, IEnumerable<IValidator<ChannelDefinition>> validators)
    : IChannelDefinitionBuilder
{

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected virtual IServiceProvider ServiceProvider { get; } = serviceProvider;

    /// <summary>
    /// Gets the services used to validate <see cref="ChannelDefinition"/>s
    /// </summary>
    protected virtual IEnumerable<IValidator<ChannelDefinition>> Validators { get; } = validators;

    /// <summary>
    /// Gets the <see cref="ChannelDefinition"/> to build
    /// </summary>
    protected virtual ChannelDefinition Channel { get; } = new();

    /// <inheritdoc/>
    public virtual IChannelDefinitionBuilder WithAddress(string address)
    {
        this.Channel.Address = address;
        return this;
    }

    /// <inheritdoc/>
    public virtual IChannelDefinitionBuilder WithDescription(string? description)
    {
        this.Channel.Description = description;
        return this;
    }

    /// <inheritdoc/>
    public virtual IChannelDefinitionBuilder WithParameter(string name, Action<IParameterDefinitionBuilder> setup)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        ArgumentNullException.ThrowIfNull(setup);
        this.Channel.Parameters ??= [];
        var builder = ActivatorUtilities.CreateInstance<ParameterDefinitionBuilder>(this.ServiceProvider);
        setup(builder);
        this.Channel.Parameters.Add(name, builder.Build());
        return this;
    }

    /// <inheritdoc/>
    public virtual IChannelDefinitionBuilder WithBinding(IChannelBindingDefinition binding)
    {
        ArgumentNullException.ThrowIfNull(binding);
        this.Channel.Bindings ??= new();
        this.Channel.Bindings.Add(binding);
        return this;
    }

    /// <inheritdoc/>
    public virtual IChannelDefinitionBuilder WithMessage(string? name, Action<IMessageDefinitionBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);

        var builder = ActivatorUtilities.CreateInstance<MessageDefinitionBuilder>(this.ServiceProvider);
        setup(builder);
        this.Channel.Messages ??= new();
        var messageDefinition = builder.Build();
        var messageName = name ?? messageDefinition.Name;
        if (!string.IsNullOrWhiteSpace(messageName) && !this.Channel.Messages.ContainsKey(messageName))
        {
            this.Channel.Messages.Add(messageName, messageDefinition);
        }

        return this;
    }

    /// <inheritdoc/>
    public virtual IChannelDefinitionBuilder WithTitle(string? title)
    {
        this.Channel.Title = title;
        return this;
    }

    /// <inheritdoc/>
    public virtual IChannelDefinitionBuilder WithSummary(string? summary)
    {
        this.Channel.Summary = summary;
        return this;
    }

    /// <inheritdoc/>
    public virtual ChannelDefinition Build()
    {
        var validationResults = this.Validators.Select(v => v.Validate(this.Channel));
        if (!validationResults.All(r => r.IsValid)) throw new ValidationException(validationResults.Where(r => !r.IsValid).SelectMany(r => r.Errors));
        return this.Channel;
    }

}
