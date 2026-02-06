// SPDX-License-Identifier: MPL-2.0

using System.Windows.Input;
using Holo.Models;

namespace Holo.Providers;

/// <summary>
/// Provides methods for registering and managing
/// commands and command contexts
/// </summary>
public interface ICommandRegistrar
{
    /// <summary>
    /// Create a context with the given <paramref name="id"/>
    /// </summary>
    /// <param name="id">Context identifier</param>
    /// <returns><see langword="true"/> if successful</returns>
    bool CreateContext(int id);

    /// <summary>
    /// Remove a context
    /// </summary>
    /// <param name="id">Context identifier</param>
    /// <returns><see langword="true"/> if successful</returns>
    bool DestroyContext(int id);

    /// <summary>
    /// Register a command
    /// </summary>
    /// <param name="contextId">Context identifier</param>
    /// <param name="info">Information about the command</param>
    /// <returns><see langword="true"/> if successful</returns>
    bool RegisterCommand(int contextId, CommandMetadata info);

    /// <summary>
    /// Get the commands registered in a context
    /// </summary>
    /// <param name="contextId">Context identifier</param>
    /// <returns>Mapping between command identifiers and instances</returns>
    IReadOnlyDictionary<string, ICommand> GetCommandsForContext(int contextId);

    /// <summary>
    /// Get information about commands registered in a context
    /// </summary>
    /// <param name="contextId">Context identifier</param>
    /// <returns>Metadata about commands in the context</returns>
    IReadOnlySet<CommandMetadata> GetMetadataForContext(int contextId);
}
