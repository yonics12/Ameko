// SPDX-License-Identifier: GPL-3.0-only

namespace Ameko.Services;

public interface IMacOsClipboardService
{
    /// <summary>
    /// Set the clipboard
    /// </summary>
    /// <param name="text">Text to set the clipboard to</param>
    void Set(string text);

    /// <summary>
    /// Get the clipboard
    /// </summary>
    /// <returns>Current clipboard content</returns>
    string Get();
}

/// <summary>
/// Local clipboard for macOS
/// </summary>
public class MacOsClipboardService : IMacOsClipboardService
{
    private string _clipboard = string.Empty;

    /// <inheritdoc />
    public void Set(string text)
    {
        _clipboard = text;
    }

    /// <inheritdoc />
    public string Get()
    {
        return _clipboard;
    }
}
