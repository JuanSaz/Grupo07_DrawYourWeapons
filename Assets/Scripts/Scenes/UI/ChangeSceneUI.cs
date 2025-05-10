using UnityEngine;

public class ChangeSceneUI: MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void ChangeScene()
    {
        SceneChanger.Instance.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        SceneChanger.Instance.QuitGame();
    }
}
