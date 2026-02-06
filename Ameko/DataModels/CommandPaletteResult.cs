// SPDX-License-Identifier: GPL-3.0-only

namespace Ameko.DataModels;

/// <summary>
/// A single result for a query in the Command Palette
/// </summary>
/// <param name="Key">Unique identifier for the result</param>
/// <param name="Name">Display name of the result</param>
public record CommandPaletteResult(string Key, string Name)
{
    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Name} ({Key})";
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }

    /// <inheritdoc />
    public virtual bool Equals(CommandPaletteResult? other)
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
public record DocumentCommandPaletteResult(string Key, string Name, int Id)
    : CommandPaletteResult(Key, Name)
{
    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Name} ({Key})";
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }

    /// <inheritdoc />
    public virtual bool Equals(DocumentCommandPaletteResult? other)
    {
        return Key == other?.Key;
    }
}
