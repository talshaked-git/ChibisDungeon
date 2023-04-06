using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Account account { get; set; }
    public GameObject[] playerPrefabs;
    public Player currentPlayer { get; set; }


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
   
        Debug.Log("Account loaded\n");
    }

    public void SaveNewPlayer(Player _player)
    {
        account.AddPlayer(_player);
        FireBaseManager.instance.SaveAccount(account);
    }
}
