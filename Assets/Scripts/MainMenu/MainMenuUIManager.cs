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
    private GameObject SettingUI;
    [SerializeField]
    private GameObject CharcterAddUI;
    [SerializeField]
    private GameObject AboutUI;
    [SerializeField]
    private Transform CharcterSelectUI;
    [SerializeField]
    private GameObject CharcterSelectPrefab;
    [SerializeField]
    private GameObject CharcterAddButtonPrefab;
    private List<GameObject> charcterSelectObjects = new List<GameObject>();
    private List<GameObject> charcterAddButtons = new List<GameObject>();

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
    private GameObject _charcterPrefabParent;
    [SerializeField]
    private GameObject[] _charcterPrefab = new GameObject[4];
    [SerializeField]
    private GameObject _NicknameTooltip;
    [SerializeField]
    private TMP_Text _NicknameTooltipText;

    private bool isSetttingUION = false;
    private bool isAboutUION = false;
    private bool isCharcterAddUION = false;

    public int currentCharcter = 0;
    private GameObject currentCharcterPrefab;

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
        StartCoroutine(UpdateCharcterScreen());
    }

    private void CleanCharacterScreen()
    {
        foreach (GameObject charcterSelectObject in charcterSelectObjects)
        {
            Destroy(charcterSelectObject);
        }

        foreach (GameObject charcterAddButton in charcterAddButtons)
        {
            Destroy(charcterAddButton);
        }
    }

    public IEnumerator UpdateCharcterScreen()
    {
        CleanCharacterScreen();
        yield return new WaitUntil(() => GameManager.instance.account != null);
        int i = 0;
        foreach (DocumentReference playerRef in GameManager.instance.account.PlayerRefs)
        {
            GameObject characterShow = Instantiate(CharcterSelectPrefab, CharcterSelectUI);
            charcterSelectObjects.Add(characterShow);
            int index = i; // Capture the current index to use inside the callback
            FireBaseManager.instance.LoadPlayer(playerRef.Id, player =>
            {
                UpdateCharTextFields(characterShow, player);
                InstantiateCharPrefab(characterShow, player);
                characterShow.GetComponentInChildren<Button>().onClick.AddListener(() => startGame(player));
            });

            i++;
        }
        


        for (int j = i; j < 4; j++)
        {
            GameObject addButtons = Instantiate(CharcterAddButtonPrefab, CharcterSelectUI);
            addButtons.GetComponentInChildren<Button>().onClick.AddListener(() => CharcterAddScreen());
            charcterAddButtons.Add(addButtons);

        }
    }

    private void startGame(Player player)
    {
        player.ListenAndUpdateDerivedStats();
        GameManager.instance.currentPlayer = player;
        GameManager.instance.ChangeScene(player.LastLocation);
        Instantiate(playerUIManagerPrefab);
    }

    private void InstantiateCharPrefab(GameObject gameObject, Player player)
    {
        int prefabIndex = 0;
        switch (player.classType)
        {
            case CharClassType.Archer:
                prefabIndex = 0;
                break;
            case CharClassType.Wizard:
                prefabIndex = 1;
                break;
            case CharClassType.Warrior:
                prefabIndex = 2;
                break;
            case CharClassType.Rogue:
                prefabIndex = 3;
                break;
            default:
                prefabIndex = 0;
                break;
        }
        //instantiate charcter prefab in charcterSelectObjects[i]
        GameObject charcterPrefab = Instantiate(_charcterPrefab[prefabIndex], gameObject.transform);
        if (player.classType == CharClassType.Archer)
        {
            // scale = new Vector3(75f, 75f, 1f);
            charcterPrefab.GetComponentInChildren<ArcherAttack>().enabled = false;
        }

        Vector3 scale = new Vector3(60f, 60f, 1f);
        charcterPrefab.transform.localScale = scale;
        charcterPrefab.transform.localPosition = new Vector3(0f, -225f, 1f);
        charcterPrefab.GetComponentInChildren<PlayerMovement>().enabled = false;
        charcterPrefab.GetComponentInChildren<EntityRenderer>().SortingLayerName = "Player";

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

    private void ClearUI()
    {
        SettingUI.SetActive(false);
        CharcterAddUI.SetActive(false);
        AboutUI.SetActive(false);
        //TODO: Clear all UI
    }


    public void SettingScreen()
    {
        ClearUI();
        isAboutUION = false;
        isCharcterAddUION = false;
        isSetttingUION = !isSetttingUION;
        SettingUI.SetActive(isSetttingUION);
    }

    public void CharcterAddScreen()
    {
        ClearUI();
        isSetttingUION = false;
        isAboutUION = false;
        isCharcterAddUION = !isCharcterAddUION;
        if (isCharcterAddUION)
        {
            currentCharcter = 0;
            _nickName.text = "";
            UpdateCharcterAddScreen();
        }
        CharcterAddUI.SetActive(isCharcterAddUION);
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
        Destroy(currentCharcterPrefab);


        currentCharcterPrefab = (GameObject)Instantiate(_charcterPrefab[currentCharcter], _charcterPrefabParent.transform.position, Quaternion.identity, _charcterPrefabParent.transform);
        if (currentCharcter == 0)
        {
            currentCharcterPrefab.transform.localScale = new Vector3(100f, 100f, 1f);
            currentCharcterPrefab.GetComponentInChildren<ArcherAttack>().enabled = false;
        }
        else
            currentCharcterPrefab.transform.localScale = new Vector3(80f, 80f, 1f);
        currentCharcterPrefab.transform.localPosition += new Vector3(0, -225f, 0);
        currentCharcterPrefab.GetComponentInChildren<PlayerMovement>().enabled = false;
    }

    public void AboutScreen()
    {
        ClearUI();
        isSetttingUION = false;
        isCharcterAddUION = false;
        isAboutUION = !isAboutUION;
        AboutUI.SetActive(isAboutUION);
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

        CharcterAddScreen();
        UpdateCharcterScreen();
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
