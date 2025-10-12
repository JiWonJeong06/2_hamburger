using UnityEngine;

[System.Serializable]
public class BossData
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

    public string dropitem; //얻을 재화
}

[System.Serializable]
public class BossDataWrapper
{
    public BossData[] boss;
}
public class BossJson : MonoBehaviour
{
    
    private static BossJson instance = null;
    public static BossJson Instance
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
        LoadBossData();
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
    public BossData[] allBossData;
    void LoadBossData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("BossTable");
        if (textAsset == null)  {return;}
        string jsonString = textAsset.text;
        BossDataWrapper bossdataWrapper = JsonUtility.FromJson<BossDataWrapper>(jsonString);
        allBossData = bossdataWrapper.boss;
    }
}