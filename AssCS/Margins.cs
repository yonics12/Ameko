// SPDX-License-Identifier: MPL-2.0

namespace AssCS;

/// <summary>
/// Margins within the video frame
/// </summary>
/// <param name="left">Left margin</param>
/// <param name="right">Right margin</param>
/// <param name="vertical">Top and bottom margin</param>
public class Margins(int left, int right, int vertical) : BindableBase
{
    private int _left = left;
    private int _right = right;
    private int _vertical = vertical;

    /// <summary>
    /// Left margin
    /// </summary>
    public int Left
    {
        get => _left;
        set => SetProperty(ref _left, value);
    }

    /// <summary>
    /// Right margin
    /// </summary>
    public int Right
    {
        get => _right;
        set => SetProperty(ref _right, value);
    }

    /// <summary>
    /// Top and bottom margin
    /// </summary>
    public int Vertical
    {
        get => _vertical;
        set => SetProperty(ref _vertical, value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Margins margins
            && _left == margins._left
            && _right == margins._right
            && _vertical == margins._vertical;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return 1;
    }

    /// <summary>
    /// Equality operator
    /// </summary>
    /// <param name="left">left margins</param>
    /// <param name="right">right margins</param>
    /// <returns><see langword="true"/> if the margins are equal</returns>
    public static bool operator ==(Margins? left, Margins? right)
    {
        return left?.Equals(right) ?? false;
    }

    /// <summary>
    /// Inequality operator
    /// </summary>
    /// <param name="left">left margins</param>
    /// <param name="right">right margins</param>
    /// <returns><see langword="true"/> if the margins are not equal</returns>
    public static bool operator !=(Margins? left, Margins? right)
    {
        return !(left == right);
    }
}
