using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using PlanB.DPF.Manager.Command.Commands;

namespace PlanB.DPF.Manager.Command;

public static class CommandInitialization
{
    /// <summary>
    /// Registers all commands in the folder
    /// </summary>
    public static void RegisterCommands(this IServiceCollection services)
    {
        var commandAssemblyPath = Assembly.GetAssembly(typeof(CommandInitialization)).Location;
        var assemblies = Directory.GetFiles(Path.GetDirectoryName(commandAssemblyPath), "Lellek.API.Command*.dll")
            .Select(AssemblyLoadContext.GetAssemblyName)
            .Select(AssemblyLoadContext.Default.LoadFromAssemblyName)
            .ToList();

        if (assemblies.Count() == 0)
        {
            throw new InvalidOperationException("Could not find Lellek.API.Command*.dll for registering command steps.");
        }

        foreach (var assembly in assemblies)
        {
            var allTypes = assembly.GetTypes().Where(x => !x.IsAbstract && !x.IsInterface).ToList();
            var types = from type in allTypes
                        where typeof(ICommand).IsAssignableFrom(type)
                        select type;
            foreach (var type in types)
            {
                services.AddTransient(type);
            }
        }
    }
}