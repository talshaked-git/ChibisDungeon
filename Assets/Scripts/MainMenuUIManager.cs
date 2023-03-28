using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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




    private bool isSetttingUION = false;
    private bool isAboutUION = false;
    private bool isCharcterAddUION = false;

    public int currentCharcter = 0;
    private GameObject currentCharcterPrefab;

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
            UpdateCharcterAddScreen();
        }
        CharcterAddUI.SetActive(isCharcterAddUION);
    }

    public void NextCharcter()
    {
        currentCharcter++;
        if (currentCharcter > 4)
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
                charcterClass =  "Archer";
                charcterLore = "In the mystical land of Elvoria, deep in the heart of the forest, there lived a young elf named Lyndria. She was born with an innate talent for archery, and from a young age, she trained tirelessly with her bow and arrows. As she grew older, Lyndria became more skilled and began to explore the vast wilderness of Elvoria. She encountered all manner of creatures, from fearsome dragons to mischievous fairies, and her archery skills proved invaluable in defending herself and others. Lyndria eventually joined the Elvorian Rangers, a group of skilled archers who protect the forest from intruders and threats. With her keen eye and precise aim, she quickly rose through the ranks and became one of the most respected members of the Rangers. Now, Lyndria is a seasoned archer with many battles under her belt. She travels throughout Elvoria, always ready to lend her skills to those in need. Her bow and arrows are her most prized possessions, and she will stop at nothing to protect her home and the people she loves.";
                CharcterStatsSTR = "15";
                CharcterStatsVIT = "10";
                CharcterStatsINT = "5";
                CharcterStatsAGI = "25";
                break;
            case 1:
                charcterClass = "Wizard";
                charcterLore = "In the ancient city of Arcanum, nestled in the foothills of the Misty Mountains, there lived a young wizard named Alaric. From a young age, Alaric showed an exceptional talent for magic, and he quickly rose through the ranks of the Arcanum Academy. As he continued to study and hone his craft, Alaric became fascinated with the ancient and powerful magic of the elves. He spent years delving into ancient tomes and studying under elven masters, eventually mastering the arcane secrets of their long-lost magic. With his newfound knowledge and power, Alaric set out to explore the world, seeking to unravel the mysteries of the arcane and unlock its full potential. He faced many dangers and challenges along the way, but his mastery of magic proved invaluable in overcoming them. Now, Alaric is a renowned wizard, respected and feared in equal measure by those who know of his exploits. He continues to travel the world, seeking new knowledge and power, and his presence is often felt in the most unexpected places. But despite his great power and knowledge, Alaric remains humble and ever-curious, always seeking to learn and grow in his mastery of the arcane.";
                CharcterStatsSTR = "5";
                CharcterStatsVIT = "10";
                CharcterStatsINT = "25";
                CharcterStatsAGI = "10";
                break;
            case 2:
                charcterClass = "Warrior";
                charcterLore = "In the rugged hills of Gorgoroth, there lived a fierce warrior named Drogan. He was born into a tribe of nomadic warriors who roamed the lands of Elvoria, battling monsters and raiding enemy villages. From a young age, Drogan showed an exceptional skill with weapons, and he quickly rose through the ranks of his tribe. He became a fierce and unstoppable force on the battlefield, wielding his broadsword and shield with deadly precision. Drogan's exploits on the battlefield soon caught the attention of the leaders of Elvoria's many kingdoms. They offered him a position as a mercenary, and he accepted, eager for new challenges and battles to fight. As a mercenary, Drogan traveled across Elvoria, fighting in countless battles and wars. His reputation as a skilled warrior and fearless fighter grew with each passing year, and he became a legend among those who knew of his exploits. Now, Drogan is a grizzled veteran, having fought in more battles than he can count. He is often called upon by the leaders of Elvoria's kingdoms to lead their armies into battle, and he never fails to deliver. Despite his many victories, Drogan remains humble and grounded, always seeking new challenges and battles to fight.";
                CharcterStatsSTR = "25";
                CharcterStatsVIT = "20";
                CharcterStatsINT = "5";
                CharcterStatsAGI = "10";
                break;
            case 3:
                charcterClass = "Rogue";
                charcterLore = "In the dark alleys of the city of Ravenhold, there lived a cunning rogue named Kira. She was born into a family of thieves and grew up learning the ways of the streets. From a young age, Kira showed an exceptional skill for thievery and deception, and she quickly became one of the most skilled rogues in Ravenhold. She could pick any lock, sneak past any guard, and disappear into the shadows without a trace. Kira's talents did not go unnoticed, and she soon caught the attention of the city's powerful underworld. She became a member of the Thieves' Guild, a secret organization that controlled much of the city's criminal activity. As a member of the Thieves' Guild, Kira took on many daring heists and missions, always managing to stay one step ahead of the law. She amassed a small fortune in stolen goods and became known throughout Ravenhold as a master thief and a force to be reckoned with. Now, Kira is a seasoned rogue, having spent years navigating the dangerous streets of Ravenhold. She continues to take on daring missions for the Thieves' Guild and remains one of their most trusted members. Despite her criminal ways, Kira has a strict code of honor and always keeps her word. She is a survivor, a fighter, and a true master of the art of thievery.";
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
        //instatiate the prefab in the center of _charcterPrefabParent
        _charcterPrefab[currentCharcter].transform.localScale = new Vector3(100f, 100f, 1f);
        currentCharcterPrefab = (GameObject)Instantiate(_charcterPrefab[currentCharcter], _charcterPrefabParent.transform.position, Quaternion.identity, _charcterPrefabParent.transform);
        currentCharcterPrefab.transform.position += new Vector3(0.2f, -2f, 0);
    }

    public void AboutScreen()
    {
        ClearUI();
        isSetttingUION = false;
        isCharcterAddUION = false;
        isAboutUION = !isAboutUION;
        AboutUI.SetActive(isAboutUION);
    }

    public void CreateCharcter(){
        string nickname =  _nickName.text;
        Player player = CreatePlayer(nickname);
        if (player == null)
        {
            Debug.Log("Player Not Created");
            return;
        }

        Debug.Log("Player Created");
        Debug.Log(player);

        
    }

    public Player CreatePlayer(string nickname){
        switch (currentCharcter)
        {
            case 0:
                return new Player(nickname,CharClassType.Archer);

            case 1:
                return new Player(nickname, CharClassType.Wizard);

            case 2:
                return new Player(nickname, CharClassType.Warrior);

            case 3:
                return new Player(nickname, CharClassType.Rogue);

            default:            
                return null;
        }
    }


}
