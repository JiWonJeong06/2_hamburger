using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private List<ICombatant> allies = new List<ICombatant>();
    private List<ICombatant> enemies = new List<ICombatant>();
    private bool isBattleRunning = false;
    public Text vicloseText;

    [Header("전투 설정")]
    public float turnInterval = 0.5f;    // 턴 간격 (공격만)
    public float meleeAttackRange = 1f;   // 근접 공격 범위

    void Update()
    {
        // 근접 캐릭터 자동 전진
        if (!isBattleRunning) return;

        MoveMeleeForward(allies, enemies);
        MoveMeleeForward(enemies, allies);
    }

    public void StartBattle()
    {
        if (isBattleRunning) return;

        allies.Clear();
        enemies.Clear();

        // 태그 기반 전투 참가자 수집
        foreach (var a in GameObject.FindGameObjectsWithTag("Ally"))
        {
            var combatant = a.GetComponent<ICombatant>();
            if (combatant != null) allies.Add(combatant);
        }

        foreach (var e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            var combatant = e.GetComponent<ICombatant>();
            if (combatant != null) enemies.Add(combatant);
        }

        foreach (var b in GameObject.FindGameObjectsWithTag("Boss"))
        {
            var combatant = b.GetComponent<ICombatant>();
            if (combatant != null) enemies.Add(combatant);
        }

        if (allies.Count == 0 || enemies.Count == 0)
        {
            Debug.LogError("❌ 전투 시작 불가: 아군 또는 적군이 없습니다.");
            return;
        }

        Debug.Log($"⚔️ 전투 시작! 아군 {allies.Count}명 vs 적 {enemies.Count}명");
        isBattleRunning = true;
        StartCoroutine(BattleLoop());
    }

    private IEnumerator BattleLoop()
    {
        while (isBattleRunning)
        {
            // --- 아군 턴 ---
            foreach (var ally in new List<ICombatant>(allies))
            {
                if (ally.IsDead)
                {
                    RemoveDead(ally, allies);
                    continue;
                }

                if (ally.Range == 0) // 근접이면 이동은 Update에서 처리
                    MeleeAttack(ally, enemies);
                else
                    RangedAttack(ally, enemies); // 원거리
            }

            yield return new WaitForSeconds(turnInterval);

            // --- 적군 턴 ---
            foreach (var enemy in new List<ICombatant>(enemies))
            {
                if (enemy.IsDead)
                {
                    RemoveDead(enemy, enemies);
                    continue;
                }

                if (enemy.Range == 0)
                    MeleeAttack(enemy, allies);
                else
                    RangedAttack(enemy, allies);
            }

            // --- 전멸 체크 ---
            allies.RemoveAll(a => a.IsDead);
            enemies.RemoveAll(e => e.IsDead);

            if (allies.Count == 0)
            {
                Debug.Log("💀 아군 전멸! 패배!");
                isBattleRunning = false;
                vicloseText.gameObject.SetActive(true);
                vicloseText.text = "패배";
                yield break;
            }

            if (enemies.Count == 0)
            {
                Debug.Log("🎉 적군 전멸! 승리!");
                isBattleRunning = false;
               vicloseText.gameObject.SetActive(true);
                vicloseText.text = "승리";
                yield break;
            }

            yield return new WaitForSeconds(turnInterval);
        }
    }

    // ---------------- 근접 캐릭터 이동 ----------------
    private void MoveMeleeForward(List<ICombatant> movers, List<ICombatant> targets)
    {
        foreach (var c in movers)
        {
            if (c.IsDead || c.Range != 0) continue; // 근접만 이동

            var obj = (c as MonoBehaviour)?.gameObject;
            if (obj == null) continue;

            // 공격 가능한 적 확인
            bool enemyInRange = false;
            foreach (var t in targets)
            {
                if (t.IsDead) continue;
                Vector3 dir = (t as MonoBehaviour).transform.position - obj.transform.position;
                if (dir.magnitude <= meleeAttackRange)
                {
                    enemyInRange = true;
                    break;
                }
            }

            if (!enemyInRange)
            {
                // 가장 가까운 적 쪽으로 전진
                var closest = GetClosestAlive(c, targets);
                if (closest != null)
                {
                    Vector3 direction = ((closest as MonoBehaviour).transform.position - obj.transform.position).normalized;
                    obj.transform.position += direction * GetSpeed(c) * Time.deltaTime;
                }
            }
        }
    }

    // ---------------- 근접 공격 ----------------
    private void MeleeAttack(ICombatant melee, List<ICombatant> targets)
    {
        var obj = (melee as MonoBehaviour)?.gameObject;
        if (obj == null) return;

        // 가까운 적 체크
        ICombatant target = null;
        foreach (var t in targets)
        {
            if (t.IsDead) continue;
            Vector3 dir = (t as MonoBehaviour).transform.position - obj.transform.position;
            if (dir.magnitude <= meleeAttackRange)
            {
                target = t;
                break;
            }
        }

        if (target != null)
        {
            target.TakeDamage(melee.Attack);
            Debug.Log($"{melee.Name} ▶ {target.Name} 근접 공격! 데미지: {melee.Attack}");
        }
    }

    // ---------------- 원거리 공격 ----------------
    private void RangedAttack(ICombatant ranged, List<ICombatant> targets)
    {
        var target = GetClosestAlive(ranged, targets);
        if (target != null)
        {
            target.TakeDamage(ranged.Attack);
            Debug.Log($"{ranged.Name} ▶ {target.Name} 원거리 공격! 데미지: {ranged.Attack}");
        }
    }

    // ---------------- 헬퍼 ----------------
    private ICombatant GetClosestAlive(ICombatant attacker, List<ICombatant> targets)
    {
        ICombatant closest = null;
        float minDist = float.MaxValue;
        Vector3 attackerPos = (attacker as MonoBehaviour).transform.position;

        foreach (var t in targets)
        {
            if (t.IsDead) continue;
            float dist = Vector3.Distance(attackerPos, (t as MonoBehaviour).transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }

        return closest;
    }

    private float GetSpeed(ICombatant c)
    {
        if (c is CharStat cs) return cs.speed;
        if (c is MonsterStat ms) return ms.speed;
        if (c is BossStat bs) return bs.speed;
        return 0f;
    }

   private void RemoveDead(ICombatant combatant, List<ICombatant> list)
{
    if (combatant == null) return;

    MonoBehaviour mb = combatant as MonoBehaviour;
    if (mb != null)
    {
        // 씬에서 즉시 제거하고 싶으면 DestroyImmediate도 가능하지만 일반적으로 Destroy로 충분
        Destroy(mb.gameObject);
    }

    // 리스트에서도 제거
    if (list.Contains(combatant))
        list.Remove(combatant);

    Debug.Log($"{combatant.Name} 사망! 씬에서 제거됨.");
}

}
