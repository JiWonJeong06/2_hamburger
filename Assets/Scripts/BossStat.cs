using UnityEngine;

public class BossStat : MonoBehaviour, ICombatant
{
    public int id; //아이디
    public string en_name; //영어 이름
    public string name; //이름
    public int range; //근, 원거리 
    public float hp; //최대체력
    public float currentHp; //현재체력
    public float attack; //공격력
    public int defense; //방어력
    public float speed; //이동속도
    public float attackspeed; //공격속도
    public string dropitem; //얻을 재화

    void Start()
    {
        LoadBossStat(id);
    }

    void LoadBossStat(int bossId)
    {
        BossData[] allBossData = BossJson.Instance.allBossData;
        BossData bossData = System.Array.Find(allBossData, boss => boss.id == bossId);

        if (bossData != null)
        {
            en_name = bossData.en_name;
            name = bossData.name;
            range = bossData.range;
            hp = bossData.hp;
            currentHp = bossData.hp;
            attack = bossData.attack;
            defense = bossData.defense;
            speed = bossData.speed;
            attackspeed = bossData.attackspeed;
            dropitem = bossData.dropitem; 
        }
        else
        {
            Debug.LogError("보스 ID에 해당하는 데이터를 찾을 수 없습니다: " + bossId);
        }
    }

    // ================= BattleManager 호환 부분 =================
    public string Name => name;
    public float HP
    {
        get => currentHp;
        set => currentHp = Mathf.Clamp(value, 0, hp);
    }
    public float Attack => attack;
    public float Defense => defense;
    public bool IsDead => currentHp <= 0;
    [SerializeField] private int _range;  // 실제 값 저장
    public int Range => _range;            // 인터페이스 구현

    public void TakeDamage(float damage)
    {
        float finalDamage = Mathf.Max(1, damage * defense*0.01f);
        currentHp -= finalDamage;
        currentHp = Mathf.Clamp(currentHp, 0, hp);
        Debug.Log($"{name}이(가) {finalDamage} 데미지를 입음! 남은 HP: {currentHp}");
         if (currentHp <= 0)
    {
        Debug.Log($"{name} 사망! 씬에서 제거됨.");
        Destroy(this.gameObject);
    }
    }
}
