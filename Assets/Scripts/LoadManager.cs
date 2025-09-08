using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public void LoadScean(string SceanName)
    {
        SceneManager.LoadScene(SceanName);
    }
}

