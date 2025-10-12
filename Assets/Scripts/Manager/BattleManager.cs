using UnityEngine;
using System.Collections;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    private bool isBattleStarted = false;

    private void Awake()
    {
        Instance = this;
    }

    public void StartBattle()
    {
        if (isBattleStarted) return;
        isBattleStarted = true;

        Debug.Log("전투 시작!");

        // 모든 유닛에게 전투 시작 신호
        foreach (UnitBase unit in FindObjectsByType<UnitBase>(FindObjectsSortMode.None))
            unit.StartBattle();

        StartCoroutine(CheckBattleEnd());
    }

    IEnumerator CheckBattleEnd()
    {
        while (isBattleStarted)
        {
            yield return new WaitForSeconds(1f);

            var players = FindObjectsByType<UnitBase>(FindObjectsSortMode.None).Where(u => u.teamType == TeamType.Player && u.hp > 0).ToList();
            var enemies = FindObjectsByType<UnitBase>(FindObjectsSortMode.None).Where(u => u.teamType == TeamType.Enemy && u.hp > 0).ToList();

            if (players.Count == 0 || enemies.Count == 0)
            {
                isBattleStarted = false;
                string winner = (players.Count > 0) ? "플레이어 승리" : "적군 승리";
                Debug.Log($"전투 종료 - {winner}");
            }
        }
    }
}
