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

namespace Neuroglia.AsyncApi.AspNetCore.UI.Models;

/// <summary>
/// Holds the data used to render a <see cref="OperationDefinition"/> view
/// </summary>
public record OperationDefinitionViewModel
    : AsyncApiDocumentViewModel
{

    /// <inheritdoc/>
    public OperationDefinitionViewModel(AsyncApiDocument document, string? channelKey, ActionType actionType, OperationDefinition operation, string operationId, List<MessageDefinition> messages) : base(document) { this.ChannelKey = channelKey; this.ActionType = actionType; this.Operation = operation; this.Messages = messages; this.OperationId = operationId; }

    /// <summary>
    /// Gets the key of the <see cref="ChannelDefinition"/> the <see cref="OperationDefinition"/> to render belongs to
    /// </summary>
    public string? ChannelKey { get; }

    /// <summary>
    /// Gets the type of the <see cref="OperationDefinition"/> to render
    /// </summary>
    public ActionType ActionType { get; }

    /// <summary>
    /// Gets the operation ID for reference.
    /// </summary>
    public string OperationId { get; }

    /// <summary>
    /// Gets the <see cref="OperationDefinition"/> to render
    /// </summary>
    public OperationDefinition Operation { get; }

    /// <summary>
    /// Gets the associated channel <see cref="MessageDefinition"/>s to render.
    /// </summary>
    public List<MessageDefinition> Messages { get; }
}
