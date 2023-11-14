using UnityEngine.SceneManagement;

public class GameManager : SingletonPersistent<GameManager>
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
