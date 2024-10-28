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

using Json.Schema;
using Json.Schema.Generation;
using Microsoft.Extensions.DependencyInjection;
using Neuroglia.AsyncApi.FluentBuilders;
using Neuroglia.AsyncApi.v3.Bindings.Http;
using Neuroglia.AsyncApi.v3;
using System.Net.Mime;

namespace Neuroglia.AsyncApi.UnitTests.Cases.Fluent;

public class FluentBuilderTests
    : IDisposable
{

    public FluentBuilderTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IAsyncApiDocumentBuilder, AsyncApiDocumentBuilder>();
        this.Services = services.BuildServiceProvider();
    }

    ServiceProvider Services { get; }

    IAsyncApiDocumentBuilder Builder => this.Services.GetRequiredService<IAsyncApiDocumentBuilder>();

    [Fact]
    public void Build_AsyncApiDocument_Should_Work()
    {
        //arrange
        var specVersion = AsyncApiSpecVersion.V2;
        var id = "fake-id";
        var title = "Fake Title";
        var version = "1.0.0-alpha1";
        var description = "Fake Description";
        var contactName = "Fake Contact Name";
        var contactUri = new Uri("https://fake-contact-uri.com");
        var contactEmail = "fake@email.com";
        var licenseName = "Apache 2.0";
        var licenseUri = new Uri("https://fake-contact-uri.com");
        var termsOfServiceUri = new Uri("https://fake.com/terms-of-service");
        var defaultContentType = MediaTypeNames.Application.Json;
        var tagName = "fake-document-tag";
        var tagDescription = "Fake Document Tag Description";
        var tagDocumentationUri = new Uri("https://fake-uri.com");
        var tagDocumentationDescription = "Fake Document Tag Documentation Description";
        var serverName = "fake-server";
        var serverUrl = new Uri("https://fake-uri.com");
        var serverProtocol = AsyncApiProtocol.Http;
        var serverHost = "fake-uri.com";
        var serverDescription = "fake-server-description";
        var serverVariableName = "fake-server-variable";
        var serverVariableDefaultValue = "fake-server-variable-default";
        var serverVariableEnumValues = new string[] { "value1", "value2", "value3" };
        var serverVariableDescription = "Fake Server Variable Description";
        var channelName = "fake-channel";
        var channelDescription = "Fake Channel Description";
        var channelParameterName = "fakeChannelParam";
        var channelParameterLocation = "/MQMD/CorrelId";
        var channelParameterDescription = "Fake Channel Param Description";
        var channelParameterSchema = new JsonSchemaBuilder().FromType<LicenseDefinition>();
        var receiveOperationId = "fake-receive-operation";
        var receiveOperationDescription = "Fake Receive Operation Description";
        var receiveOperationSummary = "Fake Receive Operation Summary";
        var sendOperationId = "fake-send-operation";
        var sendOperationDescription = "Fake Send Operation Description";
        var sendOperationSummary = "Fake Send Operation Summary";

        //act
        var document = this.Builder
            .WithSpecVersion(specVersion)
            .WithId(id)
            .WithTitle(title)
            .WithVersion(version)
            .WithDescription(description)
            .WithContact(contactName, contactUri, contactEmail)
            .WithLicense(licenseName, licenseUri)
            .WithTermsOfService(termsOfServiceUri)
            .WithDefaultContentType(defaultContentType)
            .WithServer(serverName, server => server
                .WithHost(serverHost)
                .WithProtocol(serverProtocol, "2")
                .WithDescription(serverDescription)
                .WithVariable(serverVariableName, variable => variable
                    .WithDefaultValue(serverVariableDefaultValue)
                    .WithEnumValues(serverVariableEnumValues)
                    .WithDescription(serverVariableDescription))
                .WithBinding(new HttpServerBindingDefinition()))
            .WithChannel(channelName, channel => channel
                .WithDescription(channelDescription)
            .WithMessage("licensedefinition-msg", message => message
                 .WithPayloadOfType<LicenseDefinition>())
            .WithMessage("contactdefinition-msg", message => message
                 .WithPayloadOfType<ContactDefinition>())
            .WithBinding(new HttpChannelBindingDefinition())
            .WithParameter(channelParameterName, parameter => parameter
                .WithLocation(channelParameterLocation)
                .WithDescription(channelParameterDescription)
                .WithSchema(channelParameterSchema)))
            .WithReceiveOperation(receiveOperationId, receive => receive
                .WithDescription(receiveOperationDescription)
                .WithSummary(receiveOperationSummary)
                .WithBinding(new HttpOperationBindingDefinition())
                .WithReferenceToChannelDefinition(channelName)
                .WithReferenceToMessageDefinition("licensedefinition-msg"))
            .WithSendOperation(sendOperationId, send => send
                .WithDescription(sendOperationDescription)
                .WithSummary(sendOperationSummary)
                .WithBinding(new HttpOperationBindingDefinition())
                .WithReferenceToChannelDefinition(channelName)
                .WithReferenceToMessageDefinition("licensedefinition-msg"))
            .WithTag(tag => tag
                .WithName(tagName)
                .WithDescription(tagDescription)
                .WithExternalDocumentation(tagDocumentationUri, tagDocumentationDescription))
            .WithExternalDocumentation(tagDocumentationUri, tagDocumentationDescription)
            .Build();

        //assert
        document.AsyncApi.Should().Be(specVersion);
        document.Id.Should().Be(id);
        document.Info.Title.Should().Be(title);
        document.Info.Version.Should().Be(version);
        document.Info.Description.Should().Be(description);
        document.Info.Contact.Should().NotBeNull();
        document.Info.Contact!.Name.Should().Be(contactName);
        document.Info.Contact!.Email.Should().Be(contactEmail);
        document.Info.Contact!.Url.Should().Be(contactUri);
        document.Info.License.Should().NotBeNull();
        document.Info.License!.Name.Should().Be(licenseName);
        document.Info.License.Url.Should().Be(licenseUri);
        document.Info.TermsOfService.Should().Be(termsOfServiceUri);
        document.Info.ExternalDocs.Should().NotBeNull();
        document.Info.ExternalDocs!.Description.Should().Be(tagDocumentationDescription);
        document.Info.ExternalDocs.Url.Should().Be(tagDocumentationUri);
        document.DefaultContentType.Should().Be(defaultContentType);

        var server = document.Servers!.SingleOrDefault();
        server.Should().NotBeNull();
        server.Key.Should().Be(serverName);
        server.Value.Host.Should().Be(serverHost);
        server.Value.Protocol.Should().Be(serverProtocol);
        server.Value.Description.Should().Be(serverDescription);
        server.Value.Bindings.Should().NotBeNull();
        server.Value.Bindings!.Http.Should().NotBeNull();

        var variable = server.Value.Variables!.SingleOrDefault();
        variable.Should().NotBeNull();
        variable.Key.Should().Be(serverVariableName);
        variable.Value.Default.Should().Be(serverVariableDefaultValue);
        variable.Value.Enum.Should().BeEquivalentTo(serverVariableEnumValues);
        variable.Value.Description.Should().Be(serverVariableDescription);

        var channel = document.Channels!.SingleOrDefault();
        channel.Should().NotBeNull();
        channel.Key.Should().Be(channelName);
        channel.Value.Messages.Should().NotBeNull();
        channel.Value.Messages.Should().HaveCount(2);
        var operations = document.Operations.Values;
        operations.Should().HaveCount(2);
        document.Operations.TryGetValue(receiveOperationId, out var receiveOperation);
        receiveOperation.Should().NotBeNull();
        receiveOperation!.Description.Should().Be(receiveOperationDescription);
        receiveOperation.Summary.Should().Be(receiveOperationSummary);
        receiveOperation.Messages.Should().NotBeNull();
        document.Operations.TryGetValue(sendOperationId, out var sendOperation);
        sendOperation.Should().NotBeNull();
        sendOperation!.Description.Should().Be(sendOperationDescription);
        sendOperation.Summary.Should().Be(sendOperationSummary);
        sendOperation.Messages.Should().NotBeNull();
        sendOperation.Messages.Should().HaveCount(1);
        channel.Value.Bindings.Should().NotBeNull();
        channel.Value.Bindings!.Http.Should().NotBeNull();

        var parameter = channel.Value.Parameters!.SingleOrDefault();
        parameter.Should().NotBeNull();
        parameter.Key.Should().Be(channelParameterName);
        parameter.Value.Location.Should().Be(channelParameterLocation);
        parameter.Value.Description.Should().Be(channelParameterDescription);
        parameter.Value.Schema.Should().NotBeNull();

        var tag = document.Info.Tags!.SingleOrDefault();
        tag.Should().NotBeNull();
        tag!.Name.Should().Be(tagName);
        tag.Description.Should().Be(tagDescription);
        tag.ExternalDocs.Should().NotBeNull();
        tag.ExternalDocs!.Description.Should().Be(tagDocumentationDescription);
        tag.ExternalDocs.Url.Should().Be(tagDocumentationUri);
    }

    void IDisposable.Dispose()
    {
        this.Services.Dispose();
        GC.SuppressFinalize(this);
    }

}
