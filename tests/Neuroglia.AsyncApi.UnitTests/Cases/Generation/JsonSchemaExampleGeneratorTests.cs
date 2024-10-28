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

using Json.Schema;
using Json.Schema.Generation;
using Microsoft.Extensions.DependencyInjection;
using Neuroglia.AsyncApi.Generation;
using Neuroglia.Serialization;
using System.Collections;

namespace Neuroglia.AsyncApi.UnitTests.Cases.Generation;

public class JsonSchemaExampleGeneratorTests
    : IDisposable
{

    public JsonSchemaExampleGeneratorTests()
    {
        var services = new ServiceCollection();
        services.AddSerialization();
        services.AddJsonSerializer();
        services.AddSingleton<IJsonSchemaExampleGenerator, JsonSchemaExampleGenerator>();
        this.ServiceProvider = services.BuildServiceProvider();
        this.ExampleGenerator = this.ServiceProvider.GetRequiredService<IJsonSchemaExampleGenerator>();
    }

    protected ServiceProvider ServiceProvider { get; }

    protected IJsonSchemaExampleGenerator ExampleGenerator { get; }

    [Fact]
    public void Generate_Example_Array_Should_Work()
    {
        //arrange
        var schema = new JsonSchemaBuilder().FromType(typeof(List<int>));

        //act
        var example = this.ExampleGenerator.GenerateExample(schema);
        var generated = ((IEnumerable)example!.Payload).OfType<int>();

        //assert
        generated.Should().NotBeNull();
        generated.Should().HaveCountGreaterThanOrEqualTo(1);
    }

    [Fact]
    public void Generate_Example_Boolean_Should_Work()
    {
        //arrange
        var schema = new JsonSchemaBuilder().FromType(typeof(bool));

        //act
        var example = this.ExampleGenerator.GenerateExample(schema);
        var generated = (bool?)example?.Payload;

        //assert
        generated.Should().NotBeNull();
    }

    [Fact]
    public void Generate_Example_Integer_Should_Work()
    {
        //arrange
        var schema = new JsonSchemaBuilder().FromType(typeof(int));

        //act
        var example = this.ExampleGenerator.GenerateExample(schema);
        var generated = (int?)example?.Payload;

        //assert
        generated.Should().NotBeNull();
    }

    [Fact]
    public void Generate_Example_Number_Should_Work()
    {
        //arrange
        var schema = new JsonSchemaBuilder().FromType(typeof(decimal));

        //act
        var example = this.ExampleGenerator.GenerateExample(schema);
        var generated = (decimal?)example?.Payload;

        //assert
        generated.Should().NotBeNull();
    }

    [Fact]
    public void Generate_Example_Object_Should_Work()
    {
        //arrange
        var schema = new JsonSchemaBuilder().FromType(typeof(object));

        //act
        var generated = this.ExampleGenerator.GenerateExample(schema);

        //assert
        generated.Should().NotBeNull();
    }

    [Fact]
    public void Generate_Example_String_Should_Work()
    {
        //arrange
        var schema = new JsonSchemaBuilder().FromType(typeof(string));

        //act
        var example = this.ExampleGenerator.GenerateExample(schema);
        var generated = (string?)example?.Payload;

        //assert
        generated.Should().NotBeNull();
    }

    void IDisposable.Dispose()
    {
        this.ServiceProvider.Dispose();
        GC.SuppressFinalize(this);
    }

}
