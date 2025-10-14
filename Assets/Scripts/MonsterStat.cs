using UnityEngine;

public class MonsterStat : MonoBehaviour
{
    public int id; //아이디
    public string en_name; //영어 이름
    public string name; //이름
    public int range; //근, 원거리 
    public float hp; //최대체력
    public float attack; //공격력
    public int defense; //방어력
    public float speed; //이동속도
    public float attackspeed; //공격속도

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
            range = monsterData.range;
            hp = monsterData.hp;
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
}
