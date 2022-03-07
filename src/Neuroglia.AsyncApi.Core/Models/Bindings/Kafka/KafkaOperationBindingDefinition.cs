﻿/*
 * Copyright © 2021 Neuroglia SPRL. All rights reserved.
 * <p>
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * http://www.apache.org/licenses/LICENSE-2.0
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using NJsonSchema;

namespace Neuroglia.AsyncApi.Models.Bindings.Kafka
{
    /// <summary>
    /// Represents the object used to configure a Kafka operation binding
    /// </summary>
    public class KafkaOperationBindingDefinition
        : KafkaBindingDefinition, IOperationBindingDefinition
    {

        /// <summary>
        /// Gets/sets the <see cref="JsonSchema"/> of the consumer group.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("groupId")]
        [YamlDotNet.Serialization.YamlMember(Alias = "groupId")]
        [System.Text.Json.Serialization.JsonPropertyName("groupId")]
        public virtual JsonSchema GroupId { get; set; }

        /// <summary>
        /// Gets/sets the <see cref="JsonSchema"/> of the consumer inside a consumer group.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("clientId")]
        [YamlDotNet.Serialization.YamlMember(Alias = "clientId")]
        [System.Text.Json.Serialization.JsonPropertyName("clientId")]
        public virtual JsonSchema ClientId { get; set; }

        /// <summary>
        /// Gets/sets the version of this binding. Defaults to 'latest'.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("bindingVersion")]
        [YamlDotNet.Serialization.YamlMember(Alias = "bindingVersion")]
        [System.Text.Json.Serialization.JsonPropertyName("bindingVersion")]
        public virtual string BindingVersion { get; set; } = "latest";

    }

}
