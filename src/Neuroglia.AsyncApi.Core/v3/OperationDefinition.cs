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

namespace Neuroglia.AsyncApi.v3;

/// <summary>
/// Represents an object used to define a Async API operation
/// </summary>
[DataContract]
public record OperationDefinition
    : OperationTraitDefinition
{

    /// <summary>
    /// Gets/sets a <see cref="List{T}"/> of traits to apply to the operation object. Traits MUST be merged into the operation object using the JSON Merge Patch algorithm in the same order they are defined here.
    /// </summary>
    [DataMember(Order = 1, Name = "action"), JsonPropertyOrder(1), JsonPropertyName("action"), YamlMember(Order = 1, Alias = "action")]
    public virtual ActionType Action { get; set; }

    /// <summary>
    /// Gets/sets a <see cref="List{T}"/> of traits to apply to the operation object. Traits MUST be merged into the operation object using the JSON Merge Patch algorithm in the same order they are defined here.
    /// </summary>
    [DataMember(Order = 2, Name = "traits"), JsonPropertyOrder(2), JsonPropertyName("traits"), YamlMember(Order = 2, Alias = "traits")]
    public virtual EquatableList<OperationTraitDefinition>? Traits { get; set; }

    /// <summary>
    /// Gets/sets message reference(s) that will be published or received with this operation.
    /// </summary>
    [DataMember(Order = 3, Name = "messages"), JsonPropertyOrder(3), JsonPropertyName("messages"), YamlMember(Order = 3, Alias = "messages")]
    public virtual EquatableList<ReferenceableComponentDefinition>? Messages { get; set; }

    /// <summary>
    /// Gets/sets a reference to the channel for this operation.
    /// </summary>
    [DataMember(Order = 4, Name = "channel"), JsonPropertyOrder(4), JsonPropertyName("channel"), YamlMember(Order = 4, Alias = "channel")]
    public virtual ReferenceableComponentDefinition? Channel { get; set; }

}