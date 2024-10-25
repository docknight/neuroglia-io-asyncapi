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

namespace Neuroglia.AsyncApi.IO;

/// <summary>
/// Defines the fundamentals of a service used to read <see cref="AsyncApiDocument"/>s
/// </summary>
public interface IAsyncApiDocumentReader
{

    /// <summary>
    /// Reads a <see cref="AsyncApiDocument"/> from the specified <see cref="Stream"/>
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to read the <see cref="AsyncApiDocument"/> from</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="AsyncApiDocument"/></returns>
    Task<AsyncApiDocument?> ReadAsync(Stream stream, CancellationToken cancellationToken = default);

}
