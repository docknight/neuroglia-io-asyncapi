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

using Neuroglia.AsyncApi.FluentBuilders;
using Neuroglia.AsyncApi.v3;

namespace Neuroglia.AsyncApi.CloudEvents;

/// <summary>
/// Defines extensions for <see cref="IOperationDefinitionBuilder"/>s
/// </summary>
public static class IAsyncApiDocumentBuilderExtensions
{

    /// <summary>
    /// Configures the <see cref="ChannelDefinition"/> to build to use the specified <see cref="MessageDefinition"/>
    /// </summary>
    /// <param name="channel">The <see cref="IChannelDefinitionBuilder"/> to configure</param>
    /// <param name="setup">An <see cref="Action{T}"/> used to setup the <see cref="MessageDefinition"/> to use</param>
    /// <returns>The configured <see cref="IOperationDefinitionBuilder"/></returns>
    public static IChannelDefinitionBuilder WithCloudEventMessage(this IChannelDefinitionBuilder channel, string messageName, Action<ICloudEventMessageDefinitionBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(channel);
        ArgumentNullException.ThrowIfNull(setup);

        channel.WithMessage(messageName, message =>
        {
            var cloudEvent = new CloudEventMessageDefinitionBuilder(message);
            setup(cloudEvent);
            cloudEvent.Build();
        });

        return channel;
    }

}