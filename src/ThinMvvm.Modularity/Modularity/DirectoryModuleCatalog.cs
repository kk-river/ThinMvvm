using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ThinMvvm.Modularity;

public class DirectoryModuleCatalog(string directoryPath) : ModuleCatalog, IModuleCatalog
{
    private readonly string _directoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));

    public override void Build(IServiceCollection services)
    {
        base.Build(services);

        if (!Directory.Exists(_directoryPath)) { return; }

        foreach (string dllPath in Directory.GetFiles(_directoryPath, "*.dll"))
        {
            Assembly assembly = Assembly.LoadFrom(dllPath);
            IEnumerable<Type> types = assembly.GetExportedTypes()
                .Where(type => typeof(IModule).IsAssignableFrom(type) && !type.IsAbstract);

            foreach (Type type in types)
            {
                if (Activator.CreateInstance(type) is IModule module)
                {
                    module.OnInitializing(services);
                }
            }
        }
    }
}
