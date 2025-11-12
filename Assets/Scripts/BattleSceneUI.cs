using UnityEngine;
using UnityEngine.UI;

public class BattleSceneUI : MonoBehaviour
{
    public BattleManager battleManager;
    

    [Header("캔버스 UI")]
    public GameObject pauseUI;
    public void Game_Start()
    {
        if (battleManager == null)
        {
            return;
        }

        Debug.Log("전투 시작 버튼 클릭됨");
        battleManager.StartBattle();

    }
    public void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("일시중지");
    }
    public void Goto_Start()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("계속하기");
    }
    public void ReStart()
    {
        Debug.Log("다시하기");
    }
    public void Settings()
    {
        Debug.Log("설정하기");
    }
    public void Quit_Game()
    {
        Debug.Log("나가기");
    }

}
