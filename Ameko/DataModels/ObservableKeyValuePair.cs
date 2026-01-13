// SPDX-License-Identifier: GPL-3.0-only

using AssCS;

namespace Ameko.DataModels;

/// <summary>
/// An observable implementation of a Key-Value Pair
/// </summary>
/// <param name="key">Key</param>
/// <param name="value">Initial value</param>
/// <typeparam name="T1">Type of <paramref name="key"/></typeparam>
/// <typeparam name="T2">Type of <paramref name="value"/></typeparam>
public class ObservableKeyValuePair<T1, T2>(T1 key, T2? value) : BindableBase
{
    /// <summary>
    /// Key
    /// </summary>
    public T1 Key { get; } = key;

    /// <summary>
    /// Value
    /// </summary>
    public T2? Value
    {
        get;
        set => SetProperty(ref field, value);
    } = value;
}
