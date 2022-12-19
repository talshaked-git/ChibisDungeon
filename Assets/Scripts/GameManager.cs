using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Account account;

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

    public void LoadAccount()
    {
        FireBaseManager.instance.LoadAccount(OnAccountLoaded);
    }

    private void OnAccountLoaded(Account _account)
    {
        account = _account;
        Debug.Log(account.UID + " loaded\n" + account.players[0].name);
    }
}
