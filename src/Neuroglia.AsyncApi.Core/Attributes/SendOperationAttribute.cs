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

namespace Neuroglia.AsyncApi;

/// <summary>
/// Represents an <see cref="Attribute"/> used to mark a method as an <see cref="OperationDefinition"/> of type <see cref="ActionType.Send"/>
/// </summary>
public class SendOperationAttribute
    : OperationAttribute
{

    /// <summary>
    /// Initializes a new <see cref="SendOperationAttribute"/>
    /// </summary>
    /// <param name="messageType">The <see cref="OperationDefinition"/>'s message type</param>
    /// <param name="channelName">The <see cref="OperationDefinition"/>'s channel name.</param>
    public SendOperationAttribute(Type messageType, string channelName) : base(ActionType.Send, messageType, channelName) { }

    /// <summary>
    /// Initializes a new <see cref="OperationAttribute"/>
    /// </summary>
    /// <param name="channelName">The <see cref="OperationDefinition"/>'s channel name.</param>
    public SendOperationAttribute(string channelName) : base(ActionType.Send, channelName) { }

}
