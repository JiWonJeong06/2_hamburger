using UnityEngine;
using System.Collections;

public class CharactorManager : MonoBehaviour
{
    private static CharactorManager instance = null;
    public static CharactorManager Instance
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

    [Header("캐릭터 수치")]
    public string unitName;
    public int damage;           // 공격력
    public float maxHealth;      // 최대 체력
    public float currentHealth;  // 현재 체력
    public int defense;          // 방어력
    public float speed;          // 이동 속도
    public bool ranged;          // 원거리 여부

    public enum Faction { Ally, Enemy }
    public Faction faction;

    [Header("Combat Target")]
    public Transform target;     // 공격할 상대

    [Header("Attack Settings")]
    public float attackCooldown = 1f; // 공격 주기
    private float attackTimer = 0f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        if (currentHealth <= 0 || target == null)
            return;

        // 근거리 이동
        if(!ranged)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }

        // 공격 타이머 갱신
        attackTimer -= Time.deltaTime;

        // 공격 범위 체크
        if(Vector3.Distance(transform.position, target.position) <= 1.5f && attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown; // 공격 쿨타임 초기화
        }
    }

    void Attack()
    {
        if(target == null) return;

        CharactorManager targetUnit = target.GetComponent<CharactorManager>();
        if(targetUnit == null) return;

        int actualDamage = damage - targetUnit.defense;
        if(actualDamage < 0) actualDamage = 0;

        targetUnit.TakeDamage(actualDamage);
        //Debug.Log(unitName + " 공격: " + targetUnit.unitName + " for " + actualDamage + " damage!");
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(unitName + " 공격: " + amount + " 현재 HP: " + currentHealth);

        if(currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log(unitName + " 사망!");
        Destroy(gameObject);
    }
}
