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

namespace Neuroglia.AsyncApi.v3;

/// <summary>
/// The definition of the reply in a request-reply operation.
/// </summary>
[DataContract]
public record OperationReplyDefinition : ReferenceableComponentDefinition
{
    /// <summary>
    /// Gets/sets an optional description of the address.
    /// </summary>
    [DataMember(Order = 1, Name = "address"), JsonPropertyOrder(1), JsonPropertyName("address"), YamlMember(Order = 1, Alias = "address")]
    public virtual OperationReplyAddressDefinition? Address { get; set; }

    /// <summary>
    /// Gets/sets a $ref pointer to the definition of the channel in which this operation is performed.
    /// </summary>
    [DataMember(Order = 2, Name = "channel"), JsonPropertyOrder(2), JsonPropertyName("channel"), YamlMember(Order = 2, Alias = "channel")]
    public virtual ReferenceableComponentDefinition? Channel { get; set; }

    /// <summary>
    /// Gets/sets a list of $ref pointers pointing to the supported Message Objects that can be processed by this operation as reply.
    /// </summary>
    [DataMember(Order = 3, Name = "messages"), JsonPropertyOrder(3), JsonPropertyName("messages"), YamlMember(Order = 3, Alias = "messages")]
    public virtual EquatableList<ReferenceableComponentDefinition>? Messages { get; set; }

}
