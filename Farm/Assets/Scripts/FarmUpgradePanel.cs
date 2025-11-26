using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FarmUpgradePanel : MonoBehaviour
{
    public static FarmUpgradePanel Instance { get; private set; }

    [Header("UI 레퍼런스")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button closeButton;          // ★ 추가

    private FarmPlot target;

    void Awake()
    {
        Instance = this;

        // 버튼 이벤트 연결
        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(OnClickUpgrade);

        if (closeButton != null)
            closeButton.onClick.AddListener(Close);       // ★ 추가

        gameObject.SetActive(false);
    }

    public void Open(FarmPlot plot)
    {
        target = plot;
        gameObject.SetActive(true);
        Refresh();
    }

    public void Close()
    {
        target = null;
        gameObject.SetActive(false);
    }

    private void Refresh()
    {
        if (target == null) return;

        if (target.IsMaxLevel)
        {
            levelText.text = $"Lv {target.CurrentLevel} (MAX)";
            timeText.text = "-";
            costText.text = "-";
            upgradeButton.interactable = false;
            return;
        }

        int cur = target.CurrentLevel;
        int next = cur + 1;
        levelText.text = $"Lv {cur} -> Lv {next}";

        float sec = target.CropData.baseGrowTimePerStage * target.CropData.stageSprites.Length;
        int min = Mathf.RoundToInt(sec / 60f);
        timeText.text = $"{min}min";

        int cost = target.GetNextUpgradeCost();
        costText.text = cost.ToString();

        upgradeButton.interactable =
            CurrencyManager.Instance != null &&
            CurrencyManager.Instance.Gold >= cost;
    }

    private void OnClickUpgrade()
    {
        if (target == null) return;

        if (target.TryUpgrade())
        {
            Refresh();
        }
    }
}
