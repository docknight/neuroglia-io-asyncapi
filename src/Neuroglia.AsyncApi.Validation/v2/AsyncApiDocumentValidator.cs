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

namespace Neuroglia.AsyncApi.Validation;

/// <summary>
/// Represents the service used to validate <see cref="AsyncApiDocument"/>s
/// </summary>
public class AsyncApiDocumentValidator
    : AbstractValidator<AsyncApiDocument>
{

    /// <summary>
    /// Initializes a new <see cref="AsyncApiDocumentValidator"/>
    /// </summary>
    public AsyncApiDocumentValidator()
    {
        this.RuleFor(d => d.AsyncApi)
            .NotEmpty();
        this.RuleFor(d => d.Info)
            .NotNull()
            .SetValidator(new InfoValidator());
        this.RuleFor(d => d.Channels)
            .NotEmpty();
        this.RuleForEach(d => d.Channels.Values)
            .SetValidator(new ChannelValidator())
            .When(d => d.Channels != null);
        this.RuleFor(d => d.Operations)
            .NotEmpty();
        this.RuleForEach(d => d.Operations.Values)
            .SetValidator(new OperationValidator())
            .When(d => d.Channels != null);
        this.RuleFor(d => d.Components!)
            .SetValidator(new ComponentsValidator());
        this.RuleForEach(d => d.Servers!.Values)
            .SetValidator(new ServerValidator())
            .When(d => d.Servers != null);
    }

}
