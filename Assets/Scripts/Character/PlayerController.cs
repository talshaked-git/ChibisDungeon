using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;

    void Start()
    {
        player = new Player();
        player = GameManager.instance.currentPlayer;
        player.SetExpListner();
    }

    private void OnDestroy()
    {
        player.RemoveExpListner();
    }

}
