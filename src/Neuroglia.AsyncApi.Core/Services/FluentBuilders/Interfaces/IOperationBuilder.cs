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
using Neuroglia.AsyncApi.Models;
using System;

namespace Neuroglia.AsyncApi.Services.FluentBuilders
{

    /// <summary>
    /// Defines the fundamentals of a service used to build <see cref="OperationDefinition"/>s
    /// </summary>
    public interface IOperationBuilder
        : IOperationTraitBuilder<IOperationBuilder, OperationDefinition>
    {

        /// <summary>
        /// Configures the <see cref="OperationDefinition"/> to build to use the specified <see cref="OperationTraitDefinition"/>
        /// </summary>
        /// <param name="setup">An <see cref="Action{T}"/> used to setup the <see cref="OperationTraitDefinition"/> to use</param>
        /// <returns>The configured <see cref="IOperationBuilder"/></returns>
        IOperationBuilder WithTrait(Action<IOperationTraitBuilder> setup);

        /// <summary>
        /// Configures the <see cref="OperationDefinition"/> to build to use the specified <see cref="MessageDefinition"/>
        /// </summary>
        /// <param name="setup">An <see cref="Action{T}"/> used to setup the <see cref="MessageDefinition"/> to use</param>
        /// <returns>The configured <see cref="IOperationBuilder"/></returns>
        IOperationBuilder UseMessage(Action<IMessageBuilder> setup);

    }

}
