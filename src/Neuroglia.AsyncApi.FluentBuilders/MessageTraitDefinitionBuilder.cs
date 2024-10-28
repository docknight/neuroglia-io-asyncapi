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

using Neuroglia.Data.Schemas.Json;

namespace Neuroglia.AsyncApi.FluentBuilders;

/// <summary>
/// Represents the base class for all <see cref="IMessageTraitDefinitionBuilder{TBuilder, TTrait}"/> implementations
/// </summary>
/// <typeparam name="TBuilder">The type of <see cref="IMessageTraitDefinitionBuilder{TBuilder, TTrait}"/> to return for chaining purposes</typeparam>
/// <typeparam name="TTrait">The type of <see cref="MessageTraitDefinition"/> to build</typeparam>
public abstract class MessageTraitDefinitionBuilder<TBuilder, TTrait>
    : IMessageTraitDefinitionBuilder<TBuilder, TTrait>
    where TBuilder : IMessageTraitDefinitionBuilder<TBuilder, TTrait>
    where TTrait : MessageTraitDefinition, new()
{

    /// <summary>
    /// Initializes a new <see cref="MessageTraitDefinitionBuilder{TBuilder, TTrait}"/>
    /// </summary>
    /// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
    /// <param name="validators">An <see cref="IEnumerable{T}"/> containing the services used to validate <see cref="MessageTraitDefinition"/>s</param>
    protected MessageTraitDefinitionBuilder(IServiceProvider serviceProvider, IEnumerable<IValidator<TTrait>> validators)
    {
        this.ServiceProvider = serviceProvider;
        this.Validators = validators;
    }

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets an <see cref="IEnumerable{T}"/> containing the services used to validate <see cref="MessageTraitDefinition"/>s
    /// </summary>
    protected IEnumerable<IValidator<TTrait>> Validators { get; }

    /// <summary>
    /// Gets the <see cref="MessageTraitDefinition"/> to configure
    /// </summary>
    protected virtual TTrait Trait { get; } = new();

    /// <inheritdoc/>
    public virtual TBuilder WithExample(MessageExample example)
    {
        ArgumentNullException.ThrowIfNull(example);
        this.Trait.Examples ??= [];
        this.Trait.Examples.Add(example);
        return (TBuilder)(object)this;
    }

    /// <inheritdoc/>
    public virtual TBuilder WithExternalDocumentation(Uri uri, string? description = null)
    {
        ArgumentNullException.ThrowIfNull(uri);
        this.Trait.ExternalDocs = new ExternalDocumentationDefinition() { Url = uri, Description = description };
        return (TBuilder)(object)this;
    }

    /// <inheritdoc/>
    public virtual TBuilder WithTag(Action<ITagDefinitionBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);
        this.Trait.Tags ??= [];
        var builder = ActivatorUtilities.CreateInstance<TagDefinitionBuilder>(this.ServiceProvider);
        setup(builder);
        this.Trait.Tags.Add(builder.Build());
        return (TBuilder)(object)this;
    }

    /// <inheritdoc/>
    public virtual TBuilder WithBinding(IMessageBindingDefinition binding)
    {
        ArgumentNullException.ThrowIfNull(binding);
        this.Trait.Bindings ??= new();
        this.Trait.Bindings.Add(binding);
        return (TBuilder)(object)this;
    }

    /// <inheritdoc/>
    public virtual TBuilder WithContentType(string contentType)
    {
        this.Trait.ContentType = contentType;
        return (TBuilder)(object)this;
    }

    /// <inheritdoc/>
    public virtual TBuilder WithCorrelationId(string location, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(location)) throw new ArgumentNullException(nameof(location));
        this.Trait.CorrelationId = new() { Location = location, Description = description };
        return (TBuilder)(object)this;
    }

    /// <inheritdoc/>
    public virtual TBuilder WithCorrelationId(Action<IRuntimeExpressionBuilder> locationSetup, string? description = null)
    {
        var expressionBuilder = new RuntimeExpressionBuilder();
        locationSetup(expressionBuilder);
        return this.WithCorrelationId(expressionBuilder.Build().ToString(), description);
    }

    /// <inheritdoc/>
    public virtual TBuilder WithDescription(string? description)
    {
        this.Trait.Description = description;
        return (TBuilder)(object)this;
    }

    /// <inheritdoc/>
    public virtual TBuilder WithHeaders<THeaders>() where THeaders : class => this.WithHeaders(typeof(THeaders));

    /// <inheritdoc/>
    public virtual TBuilder WithHeaders(Type headersType)
    {
        ArgumentNullException.ThrowIfNull(headersType);
        return this.WithHeaders(new JsonSchemaBuilder().FromType(headersType, JsonSchemaGeneratorConfiguration.Default));
    }

    /// <inheritdoc/>
    public virtual TBuilder WithHeaders(JsonSchema headersSchema)
    {
        this.Trait.Headers = headersSchema ?? throw new ArgumentNullException(nameof(headersSchema));
        return (TBuilder)(object)this;
    }

    /// <inheritdoc/>
    public virtual TBuilder WithName(string name)
    {
        this.Trait.Name = name;
        return (TBuilder)(object)this;
    }

    /// <inheritdoc/>
    public virtual TBuilder WithSummary(string summary)
    {
        this.Trait.Summary = summary;
        return (TBuilder)(object)this;
    }

    /// <inheritdoc/>
    public virtual TBuilder WithTitle(string title)
    {
        this.Trait.Title = title;
        return (TBuilder)(object)this;
    }

    /// <inheritdoc/>
    public virtual TTrait Build()
    {
        var validationResults = this.Validators.Select(v => v.Validate(this.Trait));
        if (!validationResults.All(r => r.IsValid)) throw new ValidationException(validationResults.Where(r => !r.IsValid).SelectMany(r => r.Errors));
        return this.Trait;
    }

}
