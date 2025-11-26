using UnityEngine;

public class FarmPlot : MonoBehaviour
{
    [Header("기본 설정")]
    public CropData cropData;              // 어떤 작물인지 (밀/양상추/토마토)
    [Range(1, 5)]
    public int level = 1;                  // 1 ~ 5

    const int MaxLevel = 5;

    [Header("연결 레퍼런스")]
    public SpriteRenderer spriteRenderer;  // 자식 XXX Sprite
    public Transform particleRoot;         // 자식 XXX ParticleRoot (빈 오브젝트)

    int currentStage = 0;      // 0,1,2,...
    float stageTimer = 0f;

    bool isReadyToHarvest = false;   // ★ 수확 가능 여부 플래그
    GameObject readyFxInstance;      // 성장완료 파티클 인스턴스

    void Start()
    {
        currentStage = 0;
        stageTimer = 0f;
        isReadyToHarvest = false;
        ApplyStageSprite();
    }

    void Update()
    {
        if (cropData == null || spriteRenderer == null)
            return;

        int maxStage = Mathf.Clamp(cropData.stageSprites.Length - 1, 0, int.MaxValue);
        if (maxStage <= 0)
            return;

        // 이미 수확 준비 완료면 더 이상 성장 X
        if (isReadyToHarvest)
            return;

        stageTimer += Time.deltaTime;
        if (stageTimer >= GetGrowTimePerStage())
        {
            stageTimer = 0f;

            // 스테이지 증가
            currentStage = Mathf.Clamp(currentStage + 1, 0, maxStage);
            ApplyStageSprite();

            // 마지막 단계에 도달하면 성장 완료 처리
            if (currentStage >= maxStage)
            {
                OnGrowComplete();
            }
        }
    }

    float GetGrowTimePerStage()
    {
        if (cropData == null)
            return 0f;

        float speedMul = 1f - 0.1f * (level - 1);
        return Mathf.Max(1f, cropData.baseGrowTimePerStage * speedMul);
    }

    void ApplyStageSprite()
    {
        if (cropData == null || spriteRenderer == null)
            return;
        if (cropData.stageSprites == null || cropData.stageSprites.Length == 0)
            return;

        int idx = Mathf.Clamp(currentStage, 0, cropData.stageSprites.Length - 1);
        spriteRenderer.sprite = cropData.stageSprites[idx];
    }

    void OnGrowComplete()
    {
        if (cropData == null)
            return;

        isReadyToHarvest = true;  // ★ 이제 수확 가능 상태

        if (cropData.readyParticlePrefab == null)
            return;

        if (readyFxInstance != null)
            return;

        Transform root = (particleRoot != null) ? particleRoot : transform;

        readyFxInstance = Instantiate(
            cropData.readyParticlePrefab,
            root.position,
            Quaternion.identity,
            root
        );
    }

    // ========================
    //   클릭 처리
    // ========================

    void OnMouseDown()
    {
        Debug.Log($"밭 클릭: {gameObject.name}, stage = {currentStage}, ready = {isReadyToHarvest}");

        // 1) 먼저 수확 시도
        if (TryHarvest())
        {
            Debug.Log("수확 성공");
            return;
        }

        // 2) 수확이 안 되는 상태면 업그레이드 패널 열기
        if (FarmUpgradePanel.Instance != null)
        {
            Debug.Log("업그레이드 패널 열기 시도");
            FarmUpgradePanel.Instance.Open(this);
        }
    }

    // ========================
    //   수확
    // ========================

    public bool TryHarvest()
    {
        if (!isReadyToHarvest)
            return false;   // 아직 다 안 자람

        if (cropData == null)
            return false;

        Debug.Log($"{cropData.displayName} 수확! 레벨 {level}");

        // TODO: 인벤토리 연동 (여기서 아이템 추가)

        // 성장 리셋
        currentStage = 0;
        stageTimer = 0f;
        isReadyToHarvest = false;
        ApplyStageSprite();

        // 파티클 제거
        if (readyFxInstance != null)
        {
            Destroy(readyFxInstance);
            readyFxInstance = null;
        }

        return true;
    }

    // ========================
    //   업그레이드 패널용 API
    // ========================

    public int CurrentLevel => level;
    public bool IsMaxLevel => level >= MaxLevel;
    public CropData CropData => cropData;

    public int GetNextUpgradeCost()
    {
        if (IsMaxLevel)
            return 0;
        if (cropData == null || cropData.upgradeCostByLevel == null || cropData.upgradeCostByLevel.Length == 0)
            return 0;

        int idx = Mathf.Clamp(level - 1, 0, cropData.upgradeCostByLevel.Length - 1);
        return cropData.upgradeCostByLevel[idx];
    }

    public bool TryUpgrade()
    {
        if (IsMaxLevel)
            return false;

        int cost = GetNextUpgradeCost();
        if (cost <= 0)
            return false;

        if (CurrencyManager.Instance == null)
        {
            Debug.LogWarning("CurrencyManager.Instance 가 없습니다.");
            return false;
        }

        if (!CurrencyManager.Instance.TrySpendGold(cost))
        {
            Debug.Log("골드가 부족합니다.");
            return false;
        }

        level++;

        // 레벨 업 시 성장 초기화
        stageTimer = 0f;
        ApplyStageSprite();

        Debug.Log($"{cropData.displayName} 업그레이드! 현재 레벨: {level}");
        return true;
    }
}
