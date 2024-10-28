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
    /// <summary>
    /// Headers and payload values for each example MUST validate against the headers or payload fields, respectively. 
    /// Example MAY also have the name and summary additional keys to provide respectively a machine-friendly name and a short summary of what the example is about.
    /// </summary>
    [DataContract]
    public record MessageExample
    {
        /// <summary>
        /// Gets/sets the example headers. The value of this field MUST validate against the Message Object's headers field.
        /// </summary>
        [DataMember(Order = 1, Name = "headers"), JsonPropertyOrder(1), JsonPropertyName("headers"), YamlMember(Order = 1, Alias = "headers")]
        public virtual object? Headers { get; set; }

        /// <summary>
        /// Gets/sets the example payload. The value of this field MUST validate against the Message Object's payload field.
        /// </summary>
        [DataMember(Order = 2, Name = "payload"), JsonPropertyOrder(2), JsonPropertyName("payload"), YamlMember(Order = 10, Alias = "payload")]
        public virtual object? Payload { get; set; }

        /// <summary>
        /// Gets/sets the machine-friendly example name.
        /// </summary>
        [DataMember(Order = 3, Name = "name"), JsonPropertyOrder(3), JsonPropertyName("name"), YamlMember(Order = 3, Alias = "name")]
        public virtual string? Name { get; set; }

        /// <summary>
        /// Gets/sets a short summary of what the example is about.
        /// </summary>
        [DataMember(Order = 4, Name = "summary"), JsonPropertyOrder(4), JsonPropertyName("summary"), YamlMember(Order = 4, Alias = "summary")]
        public virtual string? Summary { get; set; }

    }
}
