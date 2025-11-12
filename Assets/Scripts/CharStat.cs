using UnityEngine;

public class CharStat : MonoBehaviour
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
            range = characterData.range;
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
}
