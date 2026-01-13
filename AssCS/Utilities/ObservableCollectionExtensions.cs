// SPDX-License-Identifier: MPL-2.0

using System.Collections.ObjectModel;

namespace AssCS.Utilities;

public static class ObservableCollectionExtensions
{
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
