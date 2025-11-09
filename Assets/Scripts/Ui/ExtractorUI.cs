using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExtractorUI : MonoBehaviour
{
    [Header("Refs")]
    public GameObject root;
    public TMP_Text amountText;
    public Image sauceIcon;

    public TMP_Text lvText;
    public Image gaugeFill;
    public TMP_Text timerText;
    public Button btnUpgrade;
    public Button btnClose;

    [Header("Positioning")]
    public RectTransform panelRt;
    public Canvas canvas;
    public Vector2 screenOffset = new Vector2(0, -120f);
    public float worldYOffset = 0.2f;

    ExtractorRuntime target;
    Camera cam;

    void Awake()
    {
        cam = Camera.main;
        if (root) root.SetActive(false);
        if (btnClose) btnClose.onClick.AddListener(Hide);
        if (btnUpgrade) btnUpgrade.onClick.AddListener(OnUpgrade);
    }

    public void Bind(ExtractorRuntime rt)
    {
        target = rt;
        if (root) root.SetActive(true);
        Refresh();
        UpdatePanelPosition(true);
    }

    public void Hide()
    {
        if (root) root.SetActive(false);
        target = null;
    }

    void Update()
    {
        if (root == null || !root.activeSelf || target == null) return;
        Refresh();
    }

    void LateUpdate()
    {
        if (root == null || !root.activeSelf || target == null) return;
        UpdatePanelPosition(false);
    }

    void Refresh()
    {
        if (target == null || target.def == null) return;
        int li = Mathf.Clamp(target.Level - 1, 0, target.def.levels.Length - 1);
        var curr = target.def.levels[li];

        bool hasNext = (li + 1) < target.def.levels.Length;
        if (lvText)
            lvText.text = $"Lv {target.Level} -> " + (hasNext ? $"Lv {target.Level + 1}" : "MAX");

        if (hasNext)
        {
            var next = target.def.levels[li + 1];
            if (amountText) amountText.text = next.upgradeCost.ToString();

            if (target.IsUpgrading)
            {
                if (timerText) timerText.text = FormatHMS(target.UpgradeRemainSec);
                if (btnUpgrade) btnUpgrade.interactable = false;
            }
            else
            {
                float mins = next.upgradeSecs / 60f; // √  °Ê ∫–
                if (timerText) timerText.text = $"{mins:0.#} min";
                if (btnUpgrade) btnUpgrade.interactable = target.CanUpgrade();
            }
        }
        else
        {
            if (amountText) amountText.text = "-";
            if (timerText) timerText.text = "MAX";
            if (btnUpgrade) btnUpgrade.interactable = false;
        }

        if (gaugeFill)
        {
            float cap = Mathf.Max(1, curr.capacity);
            gaugeFill.fillAmount = Mathf.Clamp01(target.Stored / cap);
        }
    }

    void UpdatePanelPosition(bool snap)
    {
        if (!panelRt || !canvas || target == null) return;

        Vector3 wpos = target.transform.position + new Vector3(0f, -worldYOffset, 0f);
        Vector2 sp = RectTransformUtility.WorldToScreenPoint(cam, wpos) + screenOffset;

        RectTransform canvasRt = canvas.transform as RectTransform;
        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRt, sp,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam,
            out local);

        panelRt.anchoredPosition = local;
    }

    void OnUpgrade()
    {
        if (target == null) return;
        target.StartUpgrade();
        Refresh();
    }

    static string FormatHMS(double sec)
    {
        int h = (int)(sec / 3600);
        int m = (int)((sec % 3600) / 60);
        int s = (int)(sec % 60);
        return $"{h:00}:{m:00}:{s:00}";
    }
}
