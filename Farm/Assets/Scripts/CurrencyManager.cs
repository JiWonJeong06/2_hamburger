using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [Header("초기 골드")]
    [SerializeField] private int startGold = 0;

    // 외부에서는 읽기만, 내부에서만 수정
    public int Gold { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Gold = startGold;
        DontDestroyOnLoad(gameObject);
    }

    // 골드 사용
    public bool TrySpendGold(int amount)
    {
        if (amount <= 0)
            return true;

        if (Gold < amount)
            return false;

        Gold -= amount;
        return true;
    }

    // 골드 추가(수익, 보상 등)
    public void AddGold(int amount)
    {
        if (amount <= 0)
            return;

        Gold += amount;
    }
}

