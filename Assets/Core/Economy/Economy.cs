using System.Collections.Generic;
using UnityEngine;

// 이미 다른 파일에 있으면 이 줄 삭제
public enum ResourceType { Source }

public static class Economy
{
    static readonly Dictionary<ResourceType, int> _store = new();

    public static int Get(ResourceType t)
    {
        return _store.TryGetValue(t, out var v) ? v : 0;
    }

    public static void Add(ResourceType t, int amount)
    {
        if (amount <= 0) return;
        _store[t] = Get(t) + amount;
        // UnityEngine.Debug.Log($"[Economy] +{amount} {t} (Total={_store[t]})");
        Debug.Log($"[Economy] +{amount} {t}, Total = {_store[t]}");
    }

    public static bool TrySpend(ResourceType t, int amount)
    {
        if (amount <= 0) return true;
        var cur = Get(t);
        if (cur < amount) return false;
        _store[t] = cur - amount;
        return true;
    }

}
