using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        PlayMusic(0);
    }

    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadSceneAsync(_sceneName);
    }

    private void PlayMusic(int clip)
    {
        AudioManager.instance.PlayMusic(clip);
    }
}
