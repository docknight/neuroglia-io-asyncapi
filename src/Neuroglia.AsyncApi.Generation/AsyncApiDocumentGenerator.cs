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

using Neuroglia.Data.Schemas.Json;

namespace Neuroglia.AsyncApi.Generation;

/// <summary>
/// Represents the default implementation of the <see cref="IAsyncApiDocumentGenerator"/> interface
/// </summary>
/// <remarks>
/// Initializes a new <see cref="AsyncApiDocumentGenerator"/>
/// </remarks>
/// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
/// <param name="exampleGenerator">The service used to generate example values based on a JSON Schema</param>
public class AsyncApiDocumentGenerator(IServiceProvider serviceProvider, IJsonSchemaExampleGenerator exampleGenerator)
    : IAsyncApiDocumentGenerator
{

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;

    /// <summary>
    /// Gets the service used to generate example values based on a JSON Schema
    /// </summary>
    protected IJsonSchemaExampleGenerator ExampleGenerator { get; } = exampleGenerator;

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<AsyncApiDocument>> GenerateAsync(IEnumerable<Type> markupTypes, AsyncApiDocumentGenerationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(markupTypes);

        var types = markupTypes
            .Select(t => t.Assembly)
            .Distinct()
            .SelectMany(t => t.GetTypes()).Where(t => t.GetCustomAttribute<AsyncApiAttribute>() != null);
        var documents = new List<AsyncApiDocument>(types.Count());
        foreach (var type in types) documents.Add(await this.GenerateDocumentForAsync(type, options, cancellationToken).ConfigureAwait(false));

        return documents;
    }

    /// <summary>
    /// Generates a new <see cref="AsyncApiDocument"/> for the specified type
    /// </summary>
    /// <param name="type">The type to generate a code-first <see cref="AsyncApiDocument"/> for</param>
    /// <param name="options">The <see cref="AsyncApiDocumentGenerationOptions"/> to use</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="AsyncApiDocument"/></returns>
    protected virtual async Task<AsyncApiDocument> GenerateDocumentForAsync(Type type, AsyncApiDocumentGenerationOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(type);

        var asyncApi = type.GetCustomAttribute<AsyncApiAttribute>() ?? throw new ArgumentException($"The specified type '{type.Name}' is not marked with the {nameof(AsyncApiAttribute)}", nameof(type));
        var builder = this.ServiceProvider.GetRequiredService<IAsyncApiDocumentBuilder>();
        options.DefaultConfiguration?.Invoke(builder);
        builder
            .WithId(asyncApi.Id!)
            .WithTitle(asyncApi.Title)
            .WithVersion(asyncApi.Version);
        if (!string.IsNullOrWhiteSpace(asyncApi.Description)) builder.WithDescription(asyncApi.Description);
        if (!string.IsNullOrWhiteSpace(asyncApi.LicenseName) && !string.IsNullOrWhiteSpace(asyncApi.LicenseUrl)) builder.WithLicense(asyncApi.LicenseName, new Uri(asyncApi.LicenseUrl, UriKind.RelativeOrAbsolute));
        if (!string.IsNullOrWhiteSpace(asyncApi.TermsOfServiceUrl)) builder.WithTermsOfService(new Uri(asyncApi.TermsOfServiceUrl, UriKind.RelativeOrAbsolute));
        if (!string.IsNullOrWhiteSpace(asyncApi.ContactName)) builder.WithContact(asyncApi.ContactName, string.IsNullOrWhiteSpace(asyncApi.ContactUrl) ? null : new Uri(asyncApi.ContactUrl, UriKind.RelativeOrAbsolute), asyncApi.ContactEmail);
        var channels = new List<ChannelAttribute>();
        foreach (var channelMethodGroup in type.GetMethods(BindingFlags.Default | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Where(m => m.GetCustomAttribute<ChannelAttribute>() != null)
            .GroupBy(m => m.GetCustomAttribute<ChannelAttribute>()!.Name))
        {
            var channel = channelMethodGroup.First().GetCustomAttribute<ChannelAttribute>()!;
            var operationMethods = type.GetMethods(BindingFlags.Default | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(m => m.GetCustomAttribute<OperationAttribute>()?.ChannelName == channel.Name);
            await ConfigureChannelForAsync(builder, channel, options, operationMethods, cancellationToken).ConfigureAwait(false);
            channels.Add(channel);
        }

        foreach (var operationMethod in type.GetMethods(BindingFlags.Default | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Where(m => m.GetCustomAttribute<OperationAttribute>() != null))
        {
            var operation = operationMethod.GetCustomAttribute<OperationAttribute>()!;
            var channel = channels.Find(channel => channel.Name == operation.ChannelName);
            if (channel == null)
            {
                throw new ArgumentException($"The specified operation '{operationMethod.Name}' is not associated with a known channel.");
            }

            var message = operationMethod.GetCustomAttribute<MessageAttribute>()!;
            var messageId = ExtractMessageName(operationMethod, operation, message);
            await ConfigureOperationForAsync(builder, operation, channel.Name, operationMethod, options, messageId, cancellationToken).ConfigureAwait(false);
        }

        return builder.Build();
    }

    /// <summary>
    /// Builds a new <see cref="ChannelDefinition"/>
    /// </summary>
    /// <param name="builder">The <see cref="IAsyncApiDocumentBuilder"/> to configure</param>
    /// <param name="channel">The attribute used to describe the <see cref="ChannelDefinition"/> to configure</param>
    /// <param name="methods">A <see cref="List{T}"/> containing the <see cref="ChannelDefinition"/>'s <see cref="OperationDefinition"/>s <see cref="MethodInfo"/>s</param>
    /// <param name="options">The <see cref="AsyncApiDocumentGenerationOptions"/> to use</param>
    /// <param name="operationMethods">Operations associated with the channel.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    protected virtual async Task ConfigureChannelForAsync(IAsyncApiDocumentBuilder builder, ChannelAttribute channel, AsyncApiDocumentGenerationOptions options, IEnumerable<MethodInfo>? operationMethods, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(channel);

        builder.WithChannel(channel.Name, channelBuilder =>
        {
            channelBuilder.WithDescription(channel.Description!);
            if (operationMethods != null)
            {
                foreach (var operationMethod in operationMethods)
                {
                    var operation = operationMethod.GetCustomAttribute<OperationAttribute>()!;
                    var message = operationMethod.GetCustomAttribute<MessageAttribute>()!;
                    string? name = ExtractMessageName(operationMethod, operation, message);

                    channelBuilder.WithMessage(name, messageBuilder => ConfigureOperationMessageFor(messageBuilder, operation, operationMethod, options));
                }
            }
        });
        await Task.CompletedTask;
    }

    private static string? ExtractMessageName(MethodInfo operationMethod, OperationAttribute operation, MessageAttribute message)
    {
        var name = message?.Name;
        if (string.IsNullOrWhiteSpace(name)) name = operation.MessageType?.Name.ToCamelCase();
        if (string.IsNullOrWhiteSpace(name))
        {
            var parameters = operationMethod.GetParameters();
            if (parameters.Length == 1 || parameters.Length == 2 && parameters[1].ParameterType == typeof(CancellationToken))
            {
                name = parameters.First().ParameterType.Name;
            }
        }

        return name;
    }

    /// <summary>
    /// Configures and builds a new <see cref="MessageDefinition"/> for the specified <see cref="OperationDefinition"/>
    /// </summary>
    /// <param name="messageBuilder">The <see cref="IMessageDefinitionBuilder"/> to configure</param>
    /// <param name="operation">The attribute used to describe the <see cref="OperationDefinition"/> to configure</param>
    /// <param name="operationMethod">The <see cref="MethodInfo"/> marked with the specified <see cref="OperationDefinition"/> attribute</param>
    /// <param name="options">The <see cref="AsyncApiDocumentGenerationOptions"/> to use</param>
    protected virtual void ConfigureOperationMessageFor(IMessageDefinitionBuilder messageBuilder, OperationAttribute operation, MethodInfo operationMethod, AsyncApiDocumentGenerationOptions options)
    {
        ArgumentNullException.ThrowIfNull(messageBuilder);
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentNullException.ThrowIfNull(operationMethod);

        var messageType = operation.MessageType;
        var parameters = operationMethod.GetParameters();
        JsonSchema messageSchema;
        if (messageType == null)
        {
            if (parameters.Length == 1 || parameters.Length == 2 && parameters[1].ParameterType == typeof(CancellationToken))
            {
                messageType = parameters.First().ParameterType;
                messageSchema = new JsonSchemaBuilder().FromType(parameters.First().ParameterType, JsonSchemaGeneratorConfiguration.Default);
            }
            else
            {
                var messageSchemaBuilder = new JsonSchemaBuilder();
                var messageSchemaProperties = new Dictionary<string, JsonSchema>();
                var requiredProperties = new List<string>();
                messageType = typeof(object);
                foreach (var parameter in parameters)
                {
                    if (parameter.TryGetCustomAttribute<ExcludeAttribute>(out _)) continue;
                    var parameterSchema = messageSchemaBuilder.FromType(parameter.ParameterType, JsonSchemaGeneratorConfiguration.Default);
                    messageSchemaProperties.Add(parameter.Name!, parameterSchema);
                    if (parameter.TryGetCustomAttribute<RequiredAttribute>(out _)|| !parameter.ParameterType.IsNullable()|| parameter.DefaultValue == DBNull.Value) requiredProperties.Add(parameter.Name!);
                }
                messageSchemaBuilder.Properties(messageSchemaProperties);
                messageSchemaBuilder.Required(requiredProperties);
                messageSchema = messageSchemaBuilder.Build();
            }
        }
        else messageSchema = new JsonSchemaBuilder().FromType(messageType, JsonSchemaGeneratorConfiguration.Default);
        messageBuilder.WithPayloadSchema(messageSchema);
        var message = operationMethod.GetCustomAttribute<MessageAttribute>();
        message ??= messageType?.GetCustomAttribute<MessageAttribute>();
        var name = message?.Name;
        if (string.IsNullOrWhiteSpace(name)) name = messageType?.Name.ToCamelCase();
        var title = message?.Title;
        if (string.IsNullOrWhiteSpace(title)) title = name?.SplitCamelCase(false, true);
        var description = message?.Description;
        if (string.IsNullOrWhiteSpace(description)) description = messageType == null ? null : XmlDocumentationHelper.SummaryOf(messageType);
        var summary = message?.Summary;
        if (string.IsNullOrWhiteSpace(summary)) summary = description;
        var contentType = message?.ContentType;
        messageBuilder
            .WithName(name!)
            .WithTitle(title!)
            .WithSummary(summary!)
            .WithDescription(description!)
            .WithContentType(contentType!);
        if(messageType != null) foreach (var tag in messageType.GetCustomAttributes<TagAttribute>()) messageBuilder.WithTag(tagBuilder => tagBuilder.WithName(tag.Name).WithDescription(tag.Description!));
        if (options == null || options.AutomaticallyGenerateExamples)
        {
            messageBuilder.WithExample(this.ExampleGenerator.GenerateExample(messageSchema, requiredPropertiesOnly: true, exampleName: "Minimal")!);
            messageBuilder.WithExample(this.ExampleGenerator.GenerateExample(messageSchema, requiredPropertiesOnly: false, exampleName: "Extended")!);
        }
    }

    /// <summary>
    /// Builds a new <see cref="OperationDefinition"/>
    /// </summary>
    /// <param name="builder">The <see cref="IAsyncApiDocumentBuilder"/> to configure</param>
    /// <param name="operation">The attribute used to describe the <see cref="OperationDefinition"/> to configure</param>
    /// <param name="channelId">Reference to the operation's channel.</param>
    /// <param name="method">The <see cref="OperationDefinition"/>'s <see cref="MethodInfo"/></param>
    /// <param name="options">The <see cref="AsyncApiDocumentGenerationOptions"/> to use</param>
    /// <param name="messageId">The operation's message reference.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    protected virtual async Task ConfigureOperationForAsync(IAsyncApiDocumentBuilder builder, OperationAttribute operation, string channelId, MethodInfo method, AsyncApiDocumentGenerationOptions options, string messageId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(operation);

        builder.WithOperation(operation.ActionType, operationBuilder =>
        {
            var operationId = operation.OperationId;
            if (string.IsNullOrWhiteSpace(operationId)) operationId = (method.Name.EndsWith("Async") ? method.Name[..^5] : method.Name).ToCamelCase();
            var description = operation.Description;
            if (string.IsNullOrWhiteSpace(description)) description = XmlDocumentationHelper.SummaryOf(method);
            var summary = operation.Summary;
            if (string.IsNullOrWhiteSpace(summary)) summary = description;
            operationBuilder
                .WithOperationId(operationId)
                .WithDescription(description!)
                .WithSummary(summary!)
                .WithReferenceToChannelDefinition(channelId)
                .WithReferenceToMessageDefinition(messageId);
            foreach (var tag in method.GetCustomAttributes<TagAttribute>()) operationBuilder.WithTag(tagBuilder => tagBuilder.WithName(tag.Name).WithDescription(tag.Description!));
        });
        await Task.CompletedTask;
    }

}
