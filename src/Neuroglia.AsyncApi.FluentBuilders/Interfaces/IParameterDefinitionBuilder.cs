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

using Neuroglia.AsyncApi.v3;

namespace Neuroglia.AsyncApi.FluentBuilders;

/// <summary>
/// Defines the fundamentals of the service used to build <see cref="ParameterDefinition"/>s
/// </summary>
public interface IParameterDefinitionBuilder
{

    /// <summary>
    /// Configures the <see cref="ParameterDefinition"/> to build to use the specified description
    /// </summary>
    /// <param name="description">The description to use</param>
    /// <returns>The configured <see cref="IParameterDefinitionBuilder"/></returns>
    IParameterDefinitionBuilder WithDescription(string? description);

    /// <summary>
    /// Sets the location of the <see cref="ParameterDefinition"/> to build
    /// </summary>
    /// <param name="location">A runtime expression that specifies the location of the parameter value</param>
    /// <returns>The configured <see cref="IParameterDefinitionBuilder"/></returns>
    IParameterDefinitionBuilder WithLocation(string location);

    /// <summary>
    /// Configures the <see cref="ParameterDefinition"/> to build to use the specified enums.
    /// </summary>
    /// <param name="enums">An enumeration of string values to be used if the substitution options are from a limited set.</param>
    /// <returns>The configured <see cref="IParameterDefinitionBuilder"/></returns>
    IParameterDefinitionBuilder WithEnum(params string[] enums);

    /// <summary>
    /// Configures the <see cref="ParameterDefinition"/> to build to use the specified examples
    /// </summary>
    /// <param name="examples">The examples to use</param>
    /// <returns>The configured <see cref="IParameterDefinitionBuilder"/></returns>
    IParameterDefinitionBuilder WithExamples(params string[] examples);

    /// <summary>
    /// Configures the <see cref="ParameterDefinition"/> to build to use the specified description
    /// </summary>
    /// <param name="default">The default value to use for substitution, and to send, if an alternate value is not supplied.</param>
    /// <returns>The configured <see cref="IParameterDefinitionBuilder"/></returns>
    IParameterDefinitionBuilder WithDefault(string @default);

    /// <summary>
    /// Builds a new <see cref="ParameterDefinition"/>
    /// </summary>
    /// <returns>A new <see cref="ParameterDefinition"/></returns>
    ParameterDefinition Build();

}
