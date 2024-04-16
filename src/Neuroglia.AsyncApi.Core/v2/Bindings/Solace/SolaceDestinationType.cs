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

using Neuroglia.Serialization.Json.Converters;
using System.ComponentModel;

namespace Neuroglia.AsyncApi.v2.Bindings.Solace;

/// <summary>
/// Enumerates all types of Solace destinations
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
[TypeConverter(typeof(EnumMemberTypeConverter))]
public enum SolaceDestinationType
{
    /// <summary>
    /// Indicates a queue, which in turn will subscribe to the topic as represented by the channel name
    /// </summary>
    [EnumMember(Value = "queue")]
    Queue = 0,
    /// <summary>
    /// Indicates a topic
    /// </summary>
    [EnumMember(Value = "topic")]
    Topic = 1
}
