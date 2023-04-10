using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePlayer : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private GameObject spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
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
        player = Instantiate(GameManager.instance.playerPrefabs[prefabIndex], transform.position, Quaternion.identity, spawnPoint.transform);
        player.GetComponent<Spriter2UnityDX.EntityRenderer>().SortingLayerName = "Player";
        player.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
