using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilty script for dictionary manipulation.
/// </summary>
public class DictionaryUtil
{
    /// <summary>
    /// Function that helps deepclone a dictionary.
    /// Code was copied from here: https://stackoverflow.com/questions/139592/what-is-the-best-way-to-clone-deep-copy-a-net-generic-dictionarystring-t
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="original"></param>
    /// <returns></returns>
    public static Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>
      (Dictionary<TKey, TValue> original) where TValue : ICloneable
    {
        Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count,
                                                                original.Comparer);
        foreach (KeyValuePair<TKey, TValue> entry in original)
        {
            ret.Add(entry.Key, (TValue)entry.Value.Clone());
        }
        return ret;
    }
}
