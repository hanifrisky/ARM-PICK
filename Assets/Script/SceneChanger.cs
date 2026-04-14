using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneName;
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ChangeSceneName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ChangeSceneIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
