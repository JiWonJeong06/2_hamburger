using UnityEngine;

public class CharStat : MonoBehaviour, ICombatant
{
    public int id; //아이디
    public string en_name; //영어 이름
    public string name; //이름
    [SerializeField] private int _range; // 근거리(0), 원거리(1)
    public float hp; //최대체력
    public float currentHp; //현재체력
    public float attack; //공격력
    public int defense; //방어력
    public float speed; //이동속도
    public float attackspeed; //공격속도
    public string RoleType; //역할군

    void Start()
    {
        LoadCharStat(id);
    }

    void LoadCharStat(int charId)
    {
        Data[] allCharacterData = CharJson.Instance.allCharacterData;
        Data characterData = System.Array.Find(allCharacterData, character => character.id == charId);

        if (characterData != null)
        {
            en_name = characterData.en_name;
            name = characterData.name;
            _range = characterData.range; // ★ 여기
            hp = characterData.hp;
            currentHp = characterData.hp;
            attack = characterData.attack;
            defense = characterData.defense;
            speed = characterData.speed;
            attackspeed = characterData.attackspeed;
            RoleType = characterData.RoleType;
        }
        else
        {
            Debug.LogError("캐릭터 ID에 해당하는 데이터를 찾을 수 없습니다: " + charId);
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
    public int Range => _range; // ★ 인터페이스 구현

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
