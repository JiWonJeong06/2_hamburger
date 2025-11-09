using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct FrameSet
{
    public string id;           // "f1","f2","f3"
    public Sprite[] frames;     // 루프 프레임
    public float fps;           // 재생 속도
}

[System.Serializable]
public struct LevelRow
{
    public int level;           // 1..N
    public int frameSetIndex;   // 0=f1,1=f2,2=f3

    public float yieldPerMin;   // 분당 생산량

    [FormerlySerializedAs("prodPerHour")]
    public float _legacyProdPerHour;  // 마이그레이션용

    public int capacity;        // 저장 한도
    public int upgradeCost;     // 업그레이드 비용
    public float upgradeSecs;   // 업그레이드 시간(초)
}

[CreateAssetMenu(menuName = "Defs/Extractor")]
public class ExtractorDef : ScriptableObject
{
    public string id = "SourceExtractor";
    public ResourceType outputType = ResourceType.Source;

    public FrameSet[] frameSets;   // f1,f2,f3
    public LevelRow[] levels;      // Lv1~Lv5

    // 시간당(old) → 분당(new) 자동 변환
    void OnValidate()
    {
        if (levels == null) return;
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].yieldPerMin <= 0f && levels[i]._legacyProdPerHour > 0f)
            {
                levels[i].yieldPerMin = levels[i]._legacyProdPerHour / 60f;
                levels[i]._legacyProdPerHour = 0f;
            }
        }
    }
}
