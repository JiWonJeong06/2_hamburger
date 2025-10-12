using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSpawner : MonoBehaviour, IPointerClickHandler
{
    [Header("소환할 프리팹")]
    public GameObject prefabToSpawn;

    private GameObject previewObject;
    private Square currentSquare; // 현재 마우스 아래 있는 Square

    private void Update()
    {
        if (previewObject == null) return;

        // 마우스 위치 계산
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        previewObject.transform.position = worldPos;

        DetectSquareUnderMouse();

        // 좌클릭 → Square 위에서만 확정
        if (Input.GetMouseButtonDown(0))
        {
            TryPlaceObject();
        }
        // 우클릭 → 취소
        else if (Input.GetMouseButtonDown(1))
        {
            CancelPlacement();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (prefabToSpawn == null) return;

        // 이미 프리뷰 중이면 제거 후 새로 생성
        if (previewObject != null)
        {
            Destroy(previewObject);
        }

        previewObject = Instantiate(prefabToSpawn);
        SetPreviewTransparency(0.5f);
    }

    private void DetectSquareUnderMouse()
    {
        // Raycast로 마우스 아래 Square 감지
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.TryGetComponent(out Square square))
        {
            currentSquare = square;
        }
        else
        {
            currentSquare = null;
        }
    }

    private void TryPlaceObject()
    {
        if (currentSquare == null)
        {
            Debug.Log("❌ Square 위가 아닙니다.");
            return;
        }

        if (!currentSquare.IsEmpty)
        {
            Debug.Log("⚠️ 이미 유닛이 배치된 Square입니다.");
            return;
        }

        // Square 중앙에 프리팹 배치
        GameObject placed = Instantiate(prefabToSpawn, currentSquare.transform.position, Quaternion.identity);
        currentSquare.PlaceObject(placed);

        Destroy(previewObject);
        previewObject = null;
        currentSquare = null;
    }

    private void CancelPlacement()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
        currentSquare = null;
    }

    private void SetPreviewTransparency(float alpha)
    {
        if (previewObject == null) return;

        SpriteRenderer[] renderers = previewObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers)
        {
            Color c = r.color;
            c.a = alpha;
            r.color = c;
        }
    }
}
