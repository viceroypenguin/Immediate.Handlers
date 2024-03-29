using Immediate.Handlers.Shared;
using Microsoft.CodeAnalysis;

namespace Immediate.Handlers.Generators.ImmediateHandlers;

public partial class ImmediateHandlersGenerator
{
	private static RenderMode TransformRenderMode(GeneratorAttributeSyntaxContext context, CancellationToken token)
		=> ParseRenderMode(context.Attributes[0]);

	private static RenderMode ParseRenderMode(AttributeData attr)
	{
		if (attr.ConstructorArguments.Length != 1)
			return RenderMode.None;

		var ca = attr.ConstructorArguments[0];
		return (RenderMode?)(int?)ca.Value ?? RenderMode.None;
	}
}
