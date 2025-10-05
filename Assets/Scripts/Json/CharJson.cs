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
}


[System.Serializable]
public class CharacterDataWrapper
{
    // JSON 파일의 키 이름("characters")과 반드시 일치해야 합니다.
    public Data[] characters;
}


public class CharJson : MonoBehaviour
{
    // 파싱된 모든 캐릭터 데이터를 저장할 배열입니다.
    // public으로 선언하여 Unity Inspector에서 확인할 수 있습니다.
    public Data[] allCharacterData;

    void Awake()
    {
        LoadCharacterData();
    }

    void LoadCharacterData()
    {
        // 1. Resources 폴더에서 JSON 파일을 TextAsset으로 불러옵니다.
        // (charactortable.json 파일이 Assets/Resources 폴더 안에 있어야 합니다.)
        TextAsset textAsset = Resources.Load<TextAsset>("CharactorTable");

        if (textAsset == null)
        {
            Debug.LogError("charactortable.json 파일을 Resources 폴더에서 찾을 수 없습니다!");
            return;
        }

        // 2. 불러온 TextAsset의 텍스트 데이터를 문자열로 변환합니다.
        string jsonString = textAsset.text;

        // 3. JsonUtility를 사용하여 JSON 문자열을 CharacterDataWrapper 객체로 역직렬화(파싱)합니다.
        CharacterDataWrapper dataWrapper = JsonUtility.FromJson<CharacterDataWrapper>(jsonString);

        // 4. 파싱된 데이터를 allCharacterData 배열에 할당합니다.
        allCharacterData = dataWrapper.characters;

        // 5. 데이터가 잘 로드되었는지 확인하기 위해 첫 번째 캐릭터의 이름을 로그로 출력합니다.
        if (allCharacterData.Length > 0)
        {
            Debug.Log("첫 번째 캐릭터 이름: " + allCharacterData[0].name);
            Debug.Log("데이터 로드 성공! 총 " + allCharacterData.Length + "개의 캐릭터 데이터를 불러왔습니다.");
        }
    }
}
