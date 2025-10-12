using UnityEngine;

[System.Serializable]
public class MonsterData
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
}

[System.Serializable]
public class MonsterDataWrapper
{
    public MonsterData[] monster;
}
public class MonsterJson : MonoBehaviour
{   
    private static MonsterJson instance = null;
    public static MonsterJson Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        LoadMonsterData();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }    

    public MonsterData[] allMonsterData;
    void LoadMonsterData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("MonsterTable");
        if (textAsset == null)  {return;}
        string jsonString = textAsset.text;
        MonsterDataWrapper monsterdataWrapper = JsonUtility.FromJson<MonsterDataWrapper>(jsonString);
        allMonsterData = monsterdataWrapper.monster;
    }
}