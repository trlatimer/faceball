using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public string playerPrefabLocation;
    public GameObject[] players;
    public Transform[] spawnPoints;
    public int alivePlayers;

    private int playersInGame;
    private PhotonView photonView;

    public PlayerInputManager inputManager;

    // instance
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        inputManager.enabled = false;
        photonView = gameObject.GetPhotonView();
    }

    private void Start()
    {
        players = new GameObject[PhotonNetwork.PlayerList.Length];
        alivePlayers = players.Length;

        photonView.RPC("InGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void InGame()
    {
        playersInGame++;

        if (PhotonNetwork.IsMasterClient && playersInGame == PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("SpawnPlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void SpawnPlayer()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity); // TODO Set specific spawn points


        // Initialize the player
        playerObj.GetPhotonView().RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
}
