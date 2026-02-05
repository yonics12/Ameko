// SPDX-License-Identifier: GPL-3.0-only

namespace Ameko.DataModels;

/// <summary>
/// A single result for a query in the Command Palette
/// </summary>
/// <param name="Key">Unique identifier for the result</param>
/// <param name="Name">Display name of the result</param>
public record CommandPaletteResult(string Key, string Name);
