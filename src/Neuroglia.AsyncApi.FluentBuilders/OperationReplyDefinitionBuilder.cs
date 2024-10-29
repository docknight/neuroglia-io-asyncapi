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

using Microsoft.Extensions.DependencyInjection;
using Neuroglia.AsyncApi.FluentBuilders.Interfaces;

namespace Neuroglia.AsyncApi.FluentBuilders;

/// <inheritdoc/>
public class OperationReplyDefinitionBuilder(IServiceProvider serviceProvider, IEnumerable<IValidator<OperationReplyDefinition>> validators) : IOperationReplyDefinitionBuilder
{
    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected virtual IServiceProvider ServiceProvider { get; } = serviceProvider;

    /// <summary>
    /// Gets the services used to validate <see cref="OperationReplyDefinition"/>s
    /// </summary>
    protected virtual IEnumerable<IValidator<OperationReplyDefinition>> Validators { get; } = validators;

    /// <summary>
    /// Gets the <see cref="OperationReplyDefinition"/> to build
    /// </summary>
    protected virtual OperationReplyDefinition Reply { get; } = new();

    /// <inheritdoc/>
    public virtual IOperationReplyDefinitionBuilder WithChannelReference(string channelId)
    {
        ArgumentNullException.ThrowIfNull(channelId);
        this.Reply.Channel = new ReferenceableComponentDefinition { Reference = $"#/channels/{channelId}" };
        return this;
    }

    /// <inheritdoc/>
    public virtual IOperationReplyDefinitionBuilder WithMessageReference(string messageId, string channelId)
    {
        ArgumentNullException.ThrowIfNull(messageId);
        this.Reply.Messages ??= [];
        this.Reply.Messages.Add(new ReferenceableComponentDefinition { Reference = $"#/channels/{channelId}/messages/{messageId}" });
        return this;
    }

    /// <inheritdoc/>
    public virtual IOperationReplyDefinitionBuilder WithAddressReference(string addressId)
    {
        ArgumentNullException.ThrowIfNull(addressId);
        this.Reply.Address = new OperationReplyAddressDefinition { Reference = $"#/components/replyAddresses/{addressId}" };
        return this;
    }

    /// <inheritdoc/>
    public virtual IOperationReplyDefinitionBuilder WithAddress(string? description, string location)
    {
        ArgumentNullException.ThrowIfNull(location);
        this.Reply.Address = new OperationReplyAddressDefinition { Description = description, Location = location };
        return this;
    }

    /// <inheritdoc/>
    public OperationReplyDefinition Build()
    {
        var validationResults = this.Validators.Select(v => v.Validate(this.Reply));
        if (!validationResults.All(r => r.IsValid)) throw new ValidationException(validationResults.Where(r => !r.IsValid).SelectMany(r => r.Errors));
        return this.Reply;
    }
}
