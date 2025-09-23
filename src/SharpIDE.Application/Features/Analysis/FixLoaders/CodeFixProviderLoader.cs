using System.Collections.Immutable;
using System.Composition.Hosting;
using System.Reflection;
using Microsoft.CodeAnalysis.CodeFixes;

namespace SharpIDE.Application.Features.Analysis.FixLoaders;

public static class CodeFixProviderLoader
{
	// Alterative way, appears to be slower
	private static List<CodeFixProvider> LoadProviders(IEnumerable<Assembly> assemblies)
	{
		// Create an assembly catalog containing your current assembly
		var configuration = new ContainerConfiguration().WithAssemblies(assemblies);

		using var container = configuration.CreateContainer();
		// Get all exported CodeFixProviders
		var providers = container.GetExports<CodeFixProvider>().ToList();
		return providers;
	}

	// https://github.com/KirillOsenkov/CodeCleanupTools/blob/main/CodeFixer/FixerLoader.cs
	public static ImmutableArray<CodeFixProvider> LoadCodeFixProviders(IEnumerable<Assembly> assemblies, string language)
	{
		return assemblies
			.SelectMany(GetConcreteTypes)
			.Where(t => typeof(CodeFixProvider).IsAssignableFrom(t))
			.Where(t => IsExportedForLanguage(t, language))
			.Select(CreateInstanceOfCodeFix)
			.OfType<CodeFixProvider>()
			.ToImmutableArray();
	}

	private static bool IsExportedForLanguage(Type codeFixProvider, string language)
	{
		var exportAttribute = codeFixProvider.GetCustomAttribute<ExportCodeFixProviderAttribute>(inherit: false);
		return exportAttribute is not null && exportAttribute.Languages.Contains(language);
	}

	private static CodeFixProvider? CreateInstanceOfCodeFix(Type codeFixProvider)
	{
		try
		{
			return (CodeFixProvider)Activator.CreateInstance(codeFixProvider)!;
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
