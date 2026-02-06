// SPDX-License-Identifier: MPL-2.0

using System.Windows.Input;
using Holo.Configuration.Keybinds;

namespace Holo.Models;

public class CommandMetadata
{
    public required ICommand Command { get; init; }
    public required string QualifiedName { get; init; }
    public required string? DefaultKey { get; init; }
    public required KeybindContext DefaultContext { get; init; }
    public required bool PaletteEnabled { get; init; }
}
