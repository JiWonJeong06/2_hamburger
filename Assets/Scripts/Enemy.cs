using UnityEngine;
// 테스트용
// 나중에 JSON 파일 이용.
public class Enemy : MonoBehaviour
{
    [Header("캐릭터(적군) 수치")]
    public int damage; //공격력
    public float maxhealth; //최대 체력
    public float currenthealth; //현재 체력
    public int defense; //방어력
    public int speed; //이동 속도
    public bool ranged; //원거리형인가?
}
