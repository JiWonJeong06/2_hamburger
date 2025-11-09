using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    public Camera cam;
    public ExtractorUI ui;

    void Awake() { if (!cam) cam = Camera.main; }

    void Update()
    {
        // UI 위면 클릭 무시
        if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            var p = cam.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.OverlapPoint((Vector2)p);
            if (!hit) return;

            var rt = hit.GetComponentInParent<ExtractorRuntime>();
            if (!rt) return;

            // 업그레이드 중이 아니고 저장≥1이면 수확을 우선
            if (!rt.IsUpgrading && Mathf.FloorToInt(rt.Stored) >= 1)
            {
                rt.Collect();
                return;
            }

            // 그 외에는 패널 열기
            ui.Bind(rt);
        }

        if (Input.GetMouseButtonDown(1))  // 우클릭 닫기
            ui.Hide();
    }
}
