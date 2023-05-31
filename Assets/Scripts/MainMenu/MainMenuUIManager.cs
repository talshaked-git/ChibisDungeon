using Firebase.Extensions;
using Firebase.Firestore;
using Spriter2UnityDX;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager instance;

    [Header("Refrences")]
    [SerializeField]
    private GameObject EmptyScreen;
    [SerializeField]
    private GameObject SettingUI;
    [SerializeField]
    private GameObject CharcterAddUI;
    [SerializeField]
    private GameObject AboutUI;
    [SerializeField]
    private DeleteCharacterDialog CharacterDeleteUI;
    [SerializeField]
    private Transform CharcterSelectUI;
    [SerializeField]
    private GameObject CharcterSelectPrefab;
    [SerializeField]
    private GameObject CharcterAddButtonPrefab;
    [SerializeField] private Sprite[] _charcterClassImages;
    private List<GameObject> panelGameObjects = new List<GameObject>();
    private ListenerRegistration _playerListener;


    [Space(5f)]
    [Header("Charcter Add UI Refrences")]
    [SerializeField]
    private TMP_InputField _nickName;
    [SerializeField]
    private TMP_Text _charcterClass;
    [SerializeField]
    private TMP_Text _charcterLore;
    [SerializeField]
    private TMP_Text _charcterStatsSTR;
    [SerializeField]
    private TMP_Text _charcterStatsAGI;
    [SerializeField]
    private TMP_Text _charcterStatsINT;
    [SerializeField]
    private TMP_Text _charcterStatsVIT;
    [SerializeField]
    private GameObject _NicknameTooltip;
    [SerializeField]
    private TMP_Text _NicknameTooltipText;


    [SerializeField]
    private Image currentCharacterImage;
    public int currentCharcter = 0;

    [SerializeField] private PlayerUIManager playerUIManagerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        ClearUI();
        MainMenuListner();
    }

    public void MainMenuListner()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference userRef = db.Collection("users").Document(GameManager.instance.account.UID);
        _playerListener = userRef.Listen(snapshot =>
        {
            if (snapshot.Exists)
            {
                Debug.Log($"Current data: {snapshot.ToDictionary()}");
                List<DocumentReference> playerRefsNew = snapshot.GetValue<List<DocumentReference>>("PlayerRefs");
  
                DestroyAllCharcterSelectUI();
                panelGameObjects = new List<GameObject>();
                int index = 0;
                foreach(DocumentReference playerRef in playerRefsNew)
                {
                    GameObject characterShow = Instantiate(CharcterSelectPrefab, CharcterSelectUI);
                    panelGameObjects.Add(characterShow);
                    index++;
                    FireBaseManager.instance.LoadPlayer(playerRef.Id, player =>
                    {
                        Image imageComponent = characterShow.transform.Find("CharcterShow").GetComponentInChildren<Image>();
                        imageComponent.sprite = GetImageByClass(player.classType);
                        imageComponent.preserveAspect = true;
                        UpdateCharTextFields(characterShow, player);
                        characterShow.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => startGame(player));
                        characterShow.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => DeleteCharacterDialog(player.CID));

                    });
                }

                for (int i = index; i < 4; i++)
                {
                    GameObject gameObject = Instantiate(CharcterAddButtonPrefab, CharcterSelectUI);
                    gameObject.GetComponentInChildren<Button>().onClick.AddListener(CharcterAddScreen);
                    panelGameObjects.Add(gameObject);
                }
            }
            else
            {
                Debug.Log($"Current data: null");
            }

        });
    }

    private void DeleteCharacterDialog(string CID)
    {
        CharacterDeleteUI.Show();
        CharacterDeleteUI.OnYesEvent += () => DeleteCharacter(CID);
    }

    private void DeleteCharacter(string CID)
    {
        FireBaseManager.instance.DeletePlayer(CID);
    }

    private void DestroyAllCharcterSelectUI()
    {
        foreach (GameObject panel in panelGameObjects)
        {
            if (panel != null)
                Destroy(panel);
        }
    }

    private Sprite GetImageByClass(CharClassType charClass)
    {
        switch (charClass)
        {
            case CharClassType.Archer:
                return _charcterClassImages[0];
            case CharClassType.Wizard:
                return _charcterClassImages[1];
            case CharClassType.Warrior:
                return _charcterClassImages[2];
            case CharClassType.Rogue:
                return _charcterClassImages[3];
            default:
                return null;
        }
    }

    private void startGame(Player player)
    {
        player.ListenAndUpdateDerivedStats();
        GameManager.instance.currentPlayer = player;
        GameManager.instance.ChangeScene(player.LastLocation);
        Instantiate(playerUIManagerPrefab);
    }

    private void UpdateCharTextFields(GameObject gameObject, Player player)
    {
        TMP_Text[] fieldsToUpdate = gameObject.GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text field in fieldsToUpdate)
        {
            if (field.name == "NicknameText")
            {
                field.text = player.name;
            }
            else if (field.name == "currentLevel")
            {
                field.text = player.Level.ToString();
            }
        }
    }

    public void ClearUI()
    {
        SettingUI.SetActive(false);
        CharcterAddUI.SetActive(false);
        AboutUI.SetActive(false);
        EmptyScreen.SetActive(false);
        CharacterDeleteUI.gameObject.SetActive(false);

    }


    public void SettingScreen()
    {
        ClearUI();
        SettingUI.SetActive(true);
        EmptyScreen.SetActive(true);
    }

    public void CharcterAddScreen()
    {
        ClearUI();
        EmptyScreen.SetActive(true);
        currentCharcter = 0;
        _nickName.text = "";
        UpdateCharcterAddScreen();
        CharcterAddUI.SetActive(true);
    }

    public void NextCharcter()
    {
        currentCharcter++;
        if (currentCharcter > 3)
        {
            currentCharcter = 0;
        }
        UpdateCharcterAddScreen();
    }

    public void PreviousCharcter()
    {
        currentCharcter--;
        if (currentCharcter < 0)
        {
            currentCharcter = 3;
        }
        UpdateCharcterAddScreen();
    }

    private void UpdateCharcterAddScreen()
    {
        string charcterClass = "";
        string charcterLore = "";
        string CharcterStatsSTR = "";
        string CharcterStatsAGI = "";
        string CharcterStatsINT = "";
        string CharcterStatsVIT = "";

        switch (currentCharcter)
        {
            case 0:
                charcterClass = "Archer";
                charcterLore = "In the mystical land of Elvoria, a young elf was born with a natural talent for archery. He trained tirelessly with his bow and arrows and eventually joined the Elvorian Rangers, a group of skilled archers who protect the forest. He is always ready to defend his home and loved ones with his prized bow and arrows.";
                CharcterStatsSTR = "15";
                CharcterStatsVIT = "10";
                CharcterStatsINT = "5";
                CharcterStatsAGI = "25";
                break;
            case 1:
                charcterClass = "Wizard";
                charcterLore = "A talented wizard from Arcanum became fascinated with the ancient magic of the elves, mastering its secrets and exploring the world. Now a renowned wizard, he continues to seek new knowledge and power, remaining humble and ever-curious in his pursuit of the arcane.";
                CharcterStatsSTR = "5";
                CharcterStatsVIT = "10";
                CharcterStatsINT = "25";
                CharcterStatsAGI = "10";
                break;
            case 2:
                charcterClass = "Warrior";
                charcterLore = "In the hills of Gorgoroth lived Drogan, a skilled warrior born into a tribe of nomadic fighters. He rose through the ranks with his broadsword and shield, catching the attention of Elvoria's leaders who offered him a position as a mercenary. Drogan traveled across the land, becoming a legend for his fearless fighting and skilled warrior ways. Now a veteran, he is often called upon to lead armies into battle and always seeks new challenges.";
                CharcterStatsSTR = "25";
                CharcterStatsVIT = "20";
                CharcterStatsINT = "5";
                CharcterStatsAGI = "10";
                break;
            case 3:
                charcterClass = "Rogue";
                charcterLore = "A skilled male thief grew up learning the ways of the streets, quickly becoming one of the most accomplished rogues in the city. He caught the attention of the powerful underworld and joined the Thieves' Guild, amassing a fortune in stolen goods and becoming known as a force to be reckoned with. Now a seasoned rogue, he remains one of the guild's most trusted members, with a strict code of honor and a masterful skillset.";
                CharcterStatsSTR = "10";
                CharcterStatsVIT = "10";
                CharcterStatsINT = "10";
                CharcterStatsAGI = "25";
                break;
            default:
                break;
        }

        _charcterClass.text = charcterClass;
        _charcterLore.text = charcterLore;
        _charcterStatsSTR.text = CharcterStatsSTR;
        _charcterStatsVIT.text = CharcterStatsVIT;
        _charcterStatsINT.text = CharcterStatsINT;
        _charcterStatsAGI.text = CharcterStatsAGI;

        currentCharacterImage.sprite = _charcterClassImages[currentCharcter];
        currentCharacterImage.preserveAspect = true;

    }

    public void AboutScreen()
    {
        ClearUI();
        EmptyScreen.SetActive(true);
        AboutUI.SetActive(true);
    }

    public void CreateCharcter()
    {
        string nickname = _nickName.text;
        //validate nickname here to not be empty and not start with a number
        if (isStringEmpty(nickname))
        {
            Debug.Log("Please Enter a Nickname");
            _NicknameTooltipText.text = "Please Enter a Nickname";
            StartCoroutine(RemoveAfterSeconds(_NicknameTooltip, 3f));
            return;
        }
        else if (isStartNumber(nickname))
        {
            Debug.Log("Nickname can't start with a number");
            _NicknameTooltipText.text = "Nickname can't start with a number";
            StartCoroutine(RemoveAfterSeconds(_NicknameTooltip, 3f));
            return;
        }


        Player newPlayer = CreatePlayer(nickname);
        if (newPlayer == null)
        {
            Debug.LogWarning("Player Not Created");
            return;
        }
        DocumentReference accountRef = GameManager.instance.account.GetAccountRef();
        newPlayer.AccountRef = accountRef;
        GameManager.instance.account.AddPlayerRef(FireBaseManager.instance.SaveNewPlayer(newPlayer));

        Debug.Log("New Player Created");
        Debug.Log(newPlayer);

        ClearUI();
    }

    public Player CreatePlayer(string nickname)
    {
        //generate a unique CID
        string _CID = Guid.NewGuid().ToString();
        switch (currentCharcter)
        {
            case 0:
                return new Player(nickname, _CID, CharClassType.Archer);

            case 1:
                return new Player(nickname, _CID, CharClassType.Wizard);

            case 2:
                return new Player(nickname, _CID, CharClassType.Warrior);

            case 3:
                return new Player(nickname, _CID, CharClassType.Rogue);

            default:
                return null;
        }
    }

    public bool isStartNumber(string str)
    {
        char c = str[0];
        return (c >= '0' && c <= '9');
    }

    public bool isStringEmpty(string str)
    {
        return str == null || str.Length == 0;
    }

    private IEnumerator RemoveAfterSeconds(GameObject obj, float seconds)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }


}
