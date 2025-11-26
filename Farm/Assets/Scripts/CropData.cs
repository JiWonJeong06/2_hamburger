using UnityEngine;

[CreateAssetMenu(menuName = "OneBurger/CropData")]
public class CropData : ScriptableObject
{
    public string cropId;          // "wheat", "lettuce", "tomato"
    public string displayName;     // "밀", "양상추", "토마토"

    [Header("성장 그래픽 (3단계 스프라이트)")]
    public Sprite[] stageSprites;  // Size = 3

    [Header("성장 시간 (1단계당, 초)")]
    public float baseGrowTimePerStage = 30f;

    [Header("레벨별 수확량")]
    public int[] harvestAmountByLevel = new int[5];

    [Header("레벨업 비용 (코인)")]
    public int[] upgradeCostByLevel = new int[5];

    [Header("준비 완료 파티클 프리팹")]
    public GameObject readyParticlePrefab;
}
