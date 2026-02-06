// SPDX-License-Identifier: GPL-3.0-only

using System.Windows.Input;

namespace Ameko.DataModels;

public abstract record CommandPaletteResult;

/// <summary>
/// A single result for a query in the Command Palette
/// </summary>
/// <param name="Key">Unique identifier for the result</param>
public record CommandCommandPaletteResult(string Key, ICommand Command) : CommandPaletteResult
{
    /// <inheritdoc />
    public override string ToString()
    {
        return Key;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }

    /// <inheritdoc />
    public virtual bool Equals(CommandCommandPaletteResult? other)
    {
        return Key == other?.Key;
    }
}

/// <summary>
/// A document result for a query in the Command Palette
/// </summary>
/// <param name="Key">Unique identifier for the result</param>
/// <param name="Name">Display name of the result</param>
/// <param name="Id">Workspace ID in the project</param>
public record DocumentCommandPaletteResult(string Key, string Name, int Id) : CommandPaletteResult
{
    /// <inheritdoc />
    public override string ToString()
    {
        if (!string.IsNullOrEmpty(Key))
            return $"{Name} ({Key})";
        return Name;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }

    /// <inheritdoc />
    public virtual bool Equals(DocumentCommandPaletteResult? other)
    {
        return Id == other?.Id;
    }
}
