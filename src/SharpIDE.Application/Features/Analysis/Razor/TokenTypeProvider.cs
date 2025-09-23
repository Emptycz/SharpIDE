using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.SemanticTokens;

namespace SharpIDE.Application.Features.Analysis.Razor;

public static class TokenTypeProvider
{
	public static string[] ConstructTokenTypes(bool supportsVsExtensions)
	{
		string[] types = [.. RazorSemanticTokensAccessor.GetTokenTypes(supportsVsExtensions), .. GetStaticFieldValues(typeof(SemanticTokenTypes))];
		//return new SemanticTokenTypes(types);
		return types;
	}

	public static string[] ConstructTokenModifiers()
	{
		string[] types = [ .. RazorSemanticTokensAccessor.GetTokenModifiers(), .. GetStaticFieldValues(typeof(SemanticTokenModifiers))];
		//return new SemanticTokenModifiers(types);
		return types;
	}

	private static ImmutableArray<string> GetStaticFieldValues(Type type)
	{
		var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Static).Select(s => s.GetValue(null)).OfType<string>().ToImmutableArray();
		return fields;
	}
}
