﻿using Immediate.Handlers.Generators.ImmediateHandlers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Immediate.Handlers.Tests;

public static class TestHelper
{
	public static GeneratorDriver GetDriver(string source, IReadOnlyList<DriverReferenceAssemblies>? options = null)
	{
		// Parse the provided string into a C# syntax tree
		var syntaxTree = CSharpSyntaxTree.ParseText(source);

		// Create a Roslyn compilation for the syntax tree.
		var compilation = CSharpCompilation.Create(
			assemblyName: "Tests",
			syntaxTrees: new[] { syntaxTree },
			references: GetReferences(options)
		);

		// Create an instance of our incremental source generator
		var generator = new ImmediateHandlersGenerator();

		// The GeneratorDriver is used to run our generator against a compilation
		GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

		// Run the source generator!
		return driver.RunGenerators(compilation);
	}

	private static List<MetadataReference> GetReferences(IReadOnlyList<DriverReferenceAssemblies>? options)
	{
		List<MetadataReference> references =
		[
			.. Basic.Reference.Assemblies.NetStandard20.References.All,
			MetadataReference.CreateFromFile("./Immediate.Handlers.Shared.dll"),
		];

		if (options is null)
		{
			return references;
		}

		foreach (var option in options)
		{
			references.AddRange(option switch
			{
				DriverReferenceAssemblies.Msdi => [
					MetadataReference.CreateFromFile("./Microsoft.Extensions.DependencyInjection.dll"),
					MetadataReference.CreateFromFile("./Microsoft.Extensions.DependencyInjection.Abstractions.dll")
				],
				_ => throw new ArgumentOutOfRangeException(nameof(options), options, null),
			});
		}

		return references;
	}
}

public enum DriverReferenceAssemblies
{
	Msdi
}
