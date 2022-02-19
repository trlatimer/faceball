using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public int id;
    public Player photonPlayer;
    public PlayerInput input;


    [PunRPC]
    public void Initialize(Player player)
    {
        id = player.ActorNumber;
        photonPlayer = player;

        // Add to players list
        //GameManager.instance.players[id - 1] = this;

        // not local player
        if (!gameObject.GetPhotonView().IsMine)
        {
            input.enabled = false;
        }
    }
}
