// SPDX-License-Identifier: MPL-2.0

using System.Collections.ObjectModel;

namespace AssCS.Utilities;

/// <summary>
/// Extensions for <see cref="ObservableCollection{T}"/>
/// </summary>
public static class ObservableCollectionExtensions
{
    /// <summary>
    /// Remove all values matching the <paramref name="predicate"/>
    /// </summary>
    /// <param name="collection">Collection to remove from</param>
    /// <param name="predicate">Determine what matches</param>
    /// <typeparam name="T">Type of list</typeparam>
    /// <returns>Number of removed items</returns>
    public static int RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> predicate)
    {
        var removables = collection.Where(predicate).ToList();
        foreach (var removable in removables)
        {
            collection.Remove(removable);
        }
        return removables.Count;
    }
}
