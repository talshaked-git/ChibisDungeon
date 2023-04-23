using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePlayer : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private string m_PortalName;
    private bool m_IsCurrentSpawnPoint = false;

    private void CheckScenePortal()
    {
        string prevScene = GameManager.instance.PrevScene;
        string currentScene = GameManager.instance.CurrentScene;
        if (prevScene == "Scene_MainMenu")
        {
            if (currentScene == "Scene_Forest_Town" && m_PortalName == "Town_SpawnPoint")
            {
                m_IsCurrentSpawnPoint = true;
            }
            else if (currentScene == "Scene_Forest_1" && m_PortalName == "Forest_To_Town")
            {
                m_IsCurrentSpawnPoint = true;
            }
        }
        else if (prevScene == "Scene_Forest_Town")
        {
            if (currentScene == "Scene_Forest_1" && m_PortalName == "Forest_To_Town")
            {
                m_IsCurrentSpawnPoint = true;
            }
        }
        else if (prevScene == "Scene_Forest_1")
        {
            if (currentScene == "Scene_Forest_Town" && m_PortalName == "Town_To_Forest")
            {
                m_IsCurrentSpawnPoint = true;
            }
        }
    }

    void Start()
    {
        CheckScenePortal();

        if (!m_IsCurrentSpawnPoint)
            return;

        int prefabIndex = 0;
        switch (GameManager.instance.currentPlayer.classType)
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
        }
        //instantiate player to this objects position
        player = Instantiate(GameManager.instance.playerPrefabs[prefabIndex], transform.position, Quaternion.identity);
        player.GetComponent<Spriter2UnityDX.EntityRenderer>().SortingLayerName = "Player";
        player.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        player.GetComponentInChildren<Rigidbody2D>().simulated = true;
        player.GetComponentInChildren<PlayerMovement>().InitComponents();
        player.GetComponentInChildren<PlayerMovement>().enabled = true;
        if (GameManager.instance.currentPlayer.classType == CharClassType.Archer)
        {
            player.GetComponentInChildren<ArcherAttack>().InitComponents();
            player.GetComponentInChildren<ArcherAttack>().enabled = true;
        }
        GameObject.Find("Follow_Camera").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = player.transform;
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            Destroy(player);
        }
    }
}
