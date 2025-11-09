using UnityEngine;

/// <summary>
/// HarvestReadyFx 하나로만 사용하는 버전.
/// 저장량이 3 이상일 때 루프 FX 활성.
/// </summary>
public class ExtractorRuntime : MonoBehaviour
{
    [Header("Data")]
    public ExtractorDef def;
    [Range(1, 99)] public int Level = 1;

    [Header("Refs")]
    public SpriteRenderer body;
    public GameObject harvestFx;     // HarvestReadyFx (루프형, Loop On, Play On Awake Off)

    public float Stored { get; private set; }
    public bool IsUpgrading { get; private set; }
    public double UpgradeRemainSec { get; private set; }

    Sprite[] frames;
    float fps;
    float lastTick;
    float animT;

    void Start()
    {
        Stored = Mathf.Clamp(Stored, 0, CurrentCap());
        LoadFrameSet(true);
        UpdateHarvestCue();
    }

    void Update()
    {
        if (IsUpgrading)
        {
            TickUpgrade();
            return;
        }

        Produce();
        Animate();
    }

    #region 생산·수확
    void Produce()
    {
        var L = def.levels[Mathf.Clamp(Level - 1, 0, def.levels.Length - 1)];
        float perSec = L.yieldPerMin / 60f;
        if (perSec <= 0f) return;

        float t = Time.time;
        float dt = t - lastTick;
        if (dt <= 0f) { lastTick = t; return; }

        Stored = Mathf.Min(Stored + perSec * dt, L.capacity);
        lastTick = t;

        UpdateHarvestCue();
    }

    public void Collect()
    {
        int take = Mathf.FloorToInt(Stored);
        if (take <= 0) return;

        Economy.Add(ResourceType.Source, take);
        Stored -= take;
        UpdateHarvestCue();
    }
    #endregion

    #region 업그레이드
    public bool CanUpgrade()
    {
        if (IsUpgrading) return false;
        int nextIndex = Level;
        if (nextIndex >= def.levels.Length) return false;
        var next = def.levels[nextIndex];
        return Economy.Get(ResourceType.Source) >= next.upgradeCost;
    }

    public void StartUpgrade()
    {
        if (!CanUpgrade()) return;

        int nextIndex = Level;
        var next = def.levels[nextIndex];
        if (!Economy.TrySpend(ResourceType.Source, next.upgradeCost)) return;

        IsUpgrading = true;
        UpgradeRemainSec = Mathf.Max(0, next.upgradeSecs);
        UpdateHarvestCue();
    }

    void TickUpgrade()
    {
        if (!IsUpgrading) return;

        UpgradeRemainSec -= Time.deltaTime;
        if (UpgradeRemainSec > 0) return;

        IsUpgrading = false;
        Level = Mathf.Min(Level + 1, def.levels.Length);
        LoadFrameSet(false);
        lastTick = Time.time;
        UpdateHarvestCue();
    }
    #endregion

    #region 프레임 애니
    void LoadFrameSet(bool force)
    {
        int li = Mathf.Clamp(Level - 1, 0, def.levels.Length - 1);
        int setIdx = def.levels[li].frameSetIndex;
        if (setIdx < 0 || setIdx >= def.frameSets.Length) { frames = null; return; }

        var set = def.frameSets[setIdx];
        frames = set.frames;
        fps = Mathf.Max(0.01f, set.fps);
        animT = 0f;

        if (frames != null && frames.Length > 0 && body)
            body.sprite = frames[0];
    }

    void Animate()
    {
        if (frames == null || frames.Length == 0 || body == null) return;
        animT += Time.deltaTime * fps;
        int idx = Mathf.FloorToInt(animT) % frames.Length;
        body.sprite = frames[idx];
    }
    #endregion

    #region 파티클 (HarvestReadyFx만)
    void UpdateHarvestCue()
    {
        if (!harvestFx) return;

        if (IsUpgrading)
        {
            StopCue();
            return;
        }

        bool on = Mathf.FloorToInt(Stored) >= 3;
        if (on) StartCue();
        else StopCue();
    }

    void StartCue()
    {
        if (!harvestFx) return;
        if (!harvestFx.activeSelf) harvestFx.SetActive(true);

        var ps = harvestFx.GetComponent<ParticleSystem>();
        if (ps && !ps.isPlaying)
        {
            ps.Clear(true);
            ps.Play(true);
        }
    }

    void StopCue()
    {
        if (!harvestFx) return;
        var ps = harvestFx.GetComponent<ParticleSystem>();
        if (ps) ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        harvestFx.SetActive(false);
    }
    #endregion

    #region 유틸
    float CurrentCap()
    {
        int li = Mathf.Clamp(Level - 1, 0, def.levels.Length - 1);
        return Mathf.Max(0, def.levels[li].capacity);
    }

    void OnMouseUpAsButton()
    {
        if (!IsUpgrading) Collect();
    }
    #endregion
}
