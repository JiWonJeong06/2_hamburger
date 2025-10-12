using UnityEngine;

public enum TeamType { Player, Enemy }

public class UnitBase : MonoBehaviour
{
    public TeamType teamType;
    public float hp;
    public float attack;
    public float range;
    public float speed;
    public float attackSpeed;

    private float attackTimer;
    private UnitBase target;
    private bool isBattleActive = false;

    public void StartBattle()
    {
        isBattleActive = true;
    }

    void Update()
    {
        if (!isBattleActive || hp <= 0) return;

        if (target == null || target.hp <= 0)
            FindTarget();

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= range)
                Attack();
            else
                MoveForward();
        }
        else
        {
            MoveForward();
        }
    }

    void FindTarget()
    {
        UnitBase[] allUnits = FindObjectsByType<UnitBase>(FindObjectsSortMode.None);
        float nearestDist = Mathf.Infinity;

        foreach (var unit in allUnits)
        {
            if (unit.teamType != teamType && unit.hp > 0)
            {
                float dist = Vector3.Distance(transform.position, unit.transform.position);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    target = unit;
                }
            }
        }
    }

    void MoveForward()
    {
        float dir = (teamType == TeamType.Player) ? 1 : -1;
        transform.Translate(Vector3.right * dir * speed * Time.deltaTime);
    }

    void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= 1f / attackSpeed)
        {
            attackTimer = 0f;
            if (target != null)
                target.TakeDamage(attack);
        }
    }

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        if (hp <= 0)
            Destroy(gameObject);
    }
}
