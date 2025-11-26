using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Dictionary<string, int> items = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(string id, int amount)
    {
        if (!items.ContainsKey(id))
            items[id] = 0;

        items[id] += amount;
        // TODO: 인벤 UI 갱신 이벤트
    }

    public int GetAmount(string id)
    {
        return items.TryGetValue(id, out int value) ? value : 0;
    }
}
