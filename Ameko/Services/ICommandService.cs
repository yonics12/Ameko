// SPDX-License-Identifier: GPL-3.0-only

using System.Collections.Generic;
using Ameko.ViewModels;
using Holo.Models;

namespace Ameko.Services;

public interface ICommandService
{
    /// <summary>
    /// Scan the commands in a <paramref name="viewModel"/>
    /// and register them to a <paramref name="contextId"/>
    /// </summary>
    /// <param name="contextId">Context identifier</param>
    /// <param name="viewModel">ViewModel to scan</param>
    void RegisterCommands(int contextId, ViewModelBase viewModel);

    /// <summary>
    /// Scan for commands in the given <paramref name="viewModel"/>
    /// </summary>
    /// <param name="viewModel">ViewModel to scan</param>
    /// <returns>Information about the commands found</returns>
    IEnumerable<CommandMetadata> DiscoverCommands(ViewModelBase viewModel);
}
