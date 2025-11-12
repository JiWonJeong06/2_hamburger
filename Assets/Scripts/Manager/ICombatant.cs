public interface ICombatant
{
    string Name { get; }       // 유닛 이름
    float HP { get; set; }     // 현재 체력
    float Attack { get; }       // 공격력
    float Defense { get; }      // 방어력
    bool IsDead { get; }        // 사망 여부
    int Range { get; }         // 공격 범위

    void TakeDamage(float damage); // 피해 받는 함수
}
