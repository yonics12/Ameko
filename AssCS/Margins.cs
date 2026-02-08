// SPDX-License-Identifier: MPL-2.0

namespace AssCS;

/// <summary>
/// Margins within the video frame
/// </summary>
public class Margins : BindableBase
{
    /// <summary>
    /// Specifies the Left margin
    /// </summary>
    public int Left
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Specifies the Right margin
    /// </summary>
    public int Right
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Specifies the vertical margin
    /// </summary>
    /// <remarks>
    /// <para>
    /// Used in <see cref="AssVersion.V400P"/> and earlier.
    /// For <see cref="AssVersion.V400PP"/> and later, use <see cref="Top"/> and <see cref="Bottom"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="Top"/>
    /// <seealso cref="Bottom"/>
    public int Vertical
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Specifies the Top margin
    /// </summary>
    /// <remarks>
    /// <para>
    /// Used in <see cref="AssVersion.V400PP"/> and later.
    /// For <see cref="AssVersion.V400P"/> and earlier, use <see cref="Vertical"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="Vertical"/>
    public int Top
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Specifies the Bottom margin
    /// </summary>
    /// <remarks>
    /// <para>
    /// Used in <see cref="AssVersion.V400PP"/> and later.
    /// For <see cref="AssVersion.V400P"/> and earlier, use <see cref="Vertical"/>.
    /// </para>
    /// </remarks>
    /// <seealso cref="Vertical"/>
    public int Bottom
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Margins margins
            && Left == margins.Left
            && Right == margins.Right
            && Vertical == margins.Vertical
            && Top == margins.Top
            && Bottom == margins.Bottom;
    }

    /// <summary>
    /// Margins within the video frame
    /// </summary>
    /// <param name="left">Left margin</param>
    /// <param name="right">Right margin</param>
    /// <param name="vertical">Vertical margin</param>
    /// <remarks>
    /// This constructor is for use with <see cref="AssVersion.V400P"/> and earlier.
    /// For <see cref="AssVersion.V400PP"/> and later, use <see cref="Margins(int, int, int, int)"/>
    /// </remarks>
    public Margins(int left, int right, int vertical)
    {
        Left = left;
        Right = right;
        Vertical = vertical;
    }

    /// <summary>
    /// Margins within the video frame
    /// </summary>
    /// <param name="left">Left margin</param>
    /// <param name="right">Right margin</param>
    /// <param name="top">Top margin</param>
    /// <param name="bottom">Bottom margin</param>
    /// <remarks>
    /// This constructor is for use with <see cref="AssVersion.V400PP"/> and later.
    /// For <see cref="AssVersion.V400P"/> and earlier, use <see cref="Margins(int, int, int)"/>
    /// </remarks>
    public Margins(int left, int right, int top, int bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
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
