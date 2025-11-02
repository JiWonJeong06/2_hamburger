using UnityEngine;

[System.Serializable]
public class Data
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
    public string RoleType; //역할군
}

[System.Serializable]
public class CharacterDataWrapper
{
    public Data[] characters;
}
public class CharJson : MonoBehaviour
{

    private static CharJson instance = null;
    public static CharJson Instance
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
        LoadCharacterData();
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

    public Data[] allCharacterData;
    void LoadCharacterData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("CharacterTable");
        if (textAsset == null)
        {
            Debug.LogError("charactortable.json 파일을 Resources 폴더에서 찾을 수 없습니다!");
            return;
        }
        string jsonString = textAsset.text;
        CharacterDataWrapper dataWrapper = JsonUtility.FromJson<CharacterDataWrapper>(jsonString);
        allCharacterData = dataWrapper.characters;
    }
}