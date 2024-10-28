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


namespace Neuroglia.AsyncApi.v3
{
    [DataContract]
    public class MultiFormatSchemaObjectDefinition
    {
        /// <summary>
        /// Gets/sets a string containing the name of the schema format used to define the message payload. 
        /// If omitted, implementations should parse the payload as a <see cref="JsonSchema"/>.
        /// When the payload is defined using a $ref to a remote file, it is RECOMMENDED the schema format includes the file encoding type to allow implementations to parse the file correctly. E.g., adding +yaml if content type is application/vnd.apache.avro results in application/vnd.apache.avro+yaml.
        /// </summary>
        [DataMember(Order = 1, Name = "schemaFormat"), JsonPropertyOrder(1), JsonPropertyName("schemaFormat"), YamlMember(Order = 1, Alias = "schemaFormat")]
        public virtual string? SchemaFormat { get; set; }

        /// <summary>
        /// Gets/sets the definition of the message payload. It can be of any type but defaults to Schema object. It must match the <see cref="MessageTraitDefinition.SchemaFormat"/>, including encoding type - e.g Avro should be inlined as either a YAML or JSON object NOT a string to be parsed as YAML or JSON.
        /// </summary>
        [DataMember(Order = 2, Name = "schema"), JsonPropertyOrder(2), JsonPropertyName("schema"), YamlMember(Order = 2, Alias = "schema")]
        public virtual object? Schema { get; set; }

    }
}
