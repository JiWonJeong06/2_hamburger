using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject unitPrefab;

    public GameObject SpawnUnit(Vector3 position, Data data, TeamType team)
    {
        GameObject unit = Instantiate(unitPrefab, position, Quaternion.identity);
        UnitBase unitBase = unit.GetComponent<UnitBase>();

        unitBase.teamType = team;
        unitBase.hp = data.hp;
        unitBase.attack = data.attack;
        unitBase.range = data.range;
        unitBase.speed = data.speed;
        unitBase.attackSpeed = data.attackspeed;

        return unit;
    }
}
