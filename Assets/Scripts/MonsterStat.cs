using UnityEngine;

public class MonsterStat : MonoBehaviour, ICombatant
{
    public int id; // 아이디
    public string en_name; // 영어 이름
    public string name; // 이름

    [SerializeField] private int _range; // 근거리(0), 원거리(1)
    public float hp; // 최대체력
    public float currentHp; // 현재체력
    public float attack; // 공격력
    public int defense; // 방어력
    public float speed; // 이동속도
    public float attackspeed; // 공격속도

    void Start()
    {
        LoadMonsterStat(id);
    }

    void LoadMonsterStat(int monsterId)
    {
        MonsterData[] allMonsterData = MonsterJson.Instance.allMonsterData;
        MonsterData monsterData = System.Array.Find(allMonsterData, monster => monster.id == monsterId);

        if (monsterData != null)
        {
            en_name = monsterData.en_name;
            name = monsterData.name;
            _range = monsterData.range;   // 백업 변수에 저장
            hp = monsterData.hp;
            currentHp = monsterData.hp;
            attack = monsterData.attack;
            defense = monsterData.defense;
            speed = monsterData.speed;
            attackspeed = monsterData.attackspeed;
        }
        else
        {
            Debug.LogError("몬스터 ID에 해당하는 데이터를 찾을 수 없습니다: " + monsterId);
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

    // 인터페이스 Range 구현
    public int Range => _range; // 자기 자신을 참조하지 않음 → StackOverflow 해결
    public void DealDamage(ICombatant target)
    {
        if (target == null || target.IsDead) return;
        Debug.Log($"{Name} ▶ {target.Name} 공격!uuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuuu 데미지: {attack}");

   

        // 즉시 데미지 적용
        target.TakeDamage(attack);

    }

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
