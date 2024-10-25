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

using Neuroglia.AsyncApi.v3;

namespace Neuroglia.AsyncApi;

/// <summary>
/// Represents an <see cref="Attribute"/> used to mark a method as an <see cref="OperationDefinition"/>
/// </summary>
/// <remarks>
/// Initializes a new <see cref="OperationAttribute"/>
/// </remarks>
/// <param name="operationType">The <see cref="OperationDefinition"/>'s type</param>
/// <param name="messageType">The <see cref="OperationDefinition"/>'s message type</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public abstract class OperationAttribute(OperationType operationType, Type? messageType)
    : Attribute
{

    /// <summary>
    /// Initializes a new <see cref="OperationAttribute"/>
    /// </summary>
    /// <param name="operationType">The <see cref="OperationDefinition"/>'s type</param>
    protected OperationAttribute(OperationType operationType) : this(operationType, null) { }

    /// <summary>
    /// Gets the <see cref="OperationDefinition"/>'s type
    /// </summary>
    public virtual OperationType OperationType { get; } = operationType;

    /// <summary>
    /// Gets the <see cref="OperationDefinition"/>'s message type
    /// </summary>
    public virtual Type? MessageType { get; } = messageType;

    /// <summary>
    /// Gets/sets the <see cref="OperationDefinition"/>'s operation id
    /// </summary>
    public virtual string? OperationId { get; set; }

    /// <summary>
    /// Gets/sets the <see cref="OperationDefinition"/>'s summary
    /// </summary>
    public virtual string? Summary { get; set; }

    /// <summary>
    /// Gets/sets the <see cref="OperationDefinition"/>'s summary
    /// </summary>
    public virtual string? Description { get; set; }

}
