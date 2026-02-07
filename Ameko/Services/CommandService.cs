// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;
using Ameko.ViewModels;
using Holo.Configuration;
using Holo.Models;
using Holo.Providers;

namespace Ameko.Services;

public class CommandService(ICommandRegistrar commandRegistrar) : ICommandService
{
    /// <inheritdoc />
    public void RegisterCommands(int contextId, ViewModelBase viewModel)
    {
        commandRegistrar.CreateContext(contextId);
        foreach (var info in DiscoverCommands(viewModel))
        {
            commandRegistrar.RegisterCommand(contextId, info);
        }

        if (contextId == -1)
            return;

        // Add MainWindow commands to the context
        foreach (var info in commandRegistrar.GetMetadataForContext(-1))
        {
            commandRegistrar.RegisterCommand(contextId, info);
        }
    }

    /// <inheritdoc />
    public IEnumerable<CommandMetadata> DiscoverCommands(ViewModelBase viewModel)
    {
        // ReSharper disable once InconsistentNaming
        var isMacOS = OperatingSystem.IsMacOS();
        const string cmd = "Cmd";
        const string ctrl = "Ctrl";

        var type = viewModel.GetType();
        var props = type.GetProperties(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        foreach (var prop in props)
        {
            if (!typeof(ICommand).IsAssignableFrom(prop.PropertyType))
                continue;

            var attrib = prop.GetCustomAttribute<CommandAttribute>();
            if (attrib is null)
                continue;

            if (prop.GetValue(viewModel) is not ICommand command)
                continue;

            var defaultKey =
                isMacOS && attrib.DefaultKey is not null
                    ? attrib.DefaultKey.Replace(ctrl, cmd)
                    : attrib.DefaultKey;

            yield return new CommandMetadata
            {
                Command = command,
                QualifiedName = attrib.QualifiedName,
                DefaultKey = defaultKey,
                DefaultContext = attrib.DefaultContext,
                PaletteEnabled = attrib.PaletteEnabled,
            };
        }
    }
}
