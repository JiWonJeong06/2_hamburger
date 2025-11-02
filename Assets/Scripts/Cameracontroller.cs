using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("카메라 이동속도")]
    public float moveSpeed = 3f;

    [Header("줌 속도")]
    public float zoomSpeed = 3f;
    public float minZoom = 3f;
    public float maxZoom = 8f;

    [Header("월드 크기 (중앙 기준)")]
    public Vector2 worldSize = new Vector2(30, 12);

    [Header("카메라 경계 패딩")]
    public float padding = 0f;

    private Camera cam;
    private Vector3 lastMousePos;
    private Rect worldBounds;

    void Start()
    {
        cam = Camera.main;
        worldBounds = new Rect(-worldSize.x / 2, -worldSize.y / 2, worldSize.x, worldSize.y);
    }

    void Update()
    {
        HandleMove();
        HandleZoom();
        ClampToBounds();
    }

    void HandleMove()
    {
        // 마우스 드래그로 이동
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            transform.Translate(new Vector3(-delta.x, -delta.y, 0) * (moveSpeed * Time.deltaTime));
            lastMousePos = Input.mousePosition;
        }
    }

    void HandleZoom()
    {
        // 마우스 휠로 줌
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
    }

    void ClampToBounds()
    {

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        float minX = worldBounds.xMin + halfWidth + padding;
        float maxX = worldBounds.xMax - halfWidth - padding;
        float minY = worldBounds.yMin + halfHeight + padding;
        float maxY = worldBounds.yMax - halfHeight - padding;
        
        Vector3 p = transform.position;
        p.x = (minX > maxX) ? worldBounds.center.x : Mathf.Clamp(p.x, minX, maxX);
        p.y = (minY > maxY) ? worldBounds.center.y : Mathf.Clamp(p.y, minY, maxY);
        transform.position = p;
    }

    void OnDrawGizmosSelected()
    {
        // 에디터에서 월드 경계를 노란색 사각형으로 표시합니다.
        // Start()에서 계산된 worldBounds를 사용하도록 수정
        if(Application.isPlaying)
        {
             Gizmos.color = Color.yellow;
             Gizmos.DrawWireCube(worldBounds.center, worldBounds.size);
        }
        else
        {
            // 플레이 모드가 아닐 때는 worldSize를 기준으로 표시
             Gizmos.color = Color.yellow;
             Gizmos.DrawWireCube(Vector3.zero, new Vector3(worldSize.x, worldSize.y, 0));
        }
    }
}
