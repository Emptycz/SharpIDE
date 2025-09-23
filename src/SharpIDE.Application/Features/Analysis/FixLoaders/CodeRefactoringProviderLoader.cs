using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis.CodeRefactorings;

namespace SharpIDE.Application.Features.Analysis.FixLoaders;

public static class CodeRefactoringProviderLoader
{
	public static ImmutableArray<CodeRefactoringProvider> LoadCodeRefactoringProviders(IEnumerable<Assembly> assemblies, string language)
	{
		return assemblies
			.SelectMany(GetConcreteTypes)
			.Where(t => typeof(CodeRefactoringProvider).IsAssignableFrom(t))
			.Where(t => IsExportedForLanguage(t, language))
			.Select(CreateInstanceOfCodeRefactoring)
			.OfType<CodeRefactoringProvider>()
			.ToImmutableArray();
	}

	private static bool IsExportedForLanguage(Type refactoringProvider, string language)
	{
		var exportAttribute = refactoringProvider.GetCustomAttribute<ExportCodeRefactoringProviderAttribute>(inherit: false);
		return exportAttribute is not null && exportAttribute.Languages.Contains(language);
	}

	private static CodeRefactoringProvider? CreateInstanceOfCodeRefactoring(Type refactoringProvider)
	{
		try
		{
			return (CodeRefactoringProvider)Activator.CreateInstance(refactoringProvider)!;
		}
		catch
		{
			return null;
		}
	}

	private static IEnumerable<Type> GetConcreteTypes(Assembly assembly)
	{
		try
		{
			var concreteTypes = assembly
				.GetTypes()
				.Where(type => !type.GetTypeInfo().IsInterface
				               && !type.GetTypeInfo().IsAbstract
				               && !type.GetTypeInfo().ContainsGenericParameters);

			// Realize the collection to ensure exceptions are caught
			return concreteTypes.ToList();
		}
		catch
		{
			return Type.EmptyTypes;
		}
	}

}
