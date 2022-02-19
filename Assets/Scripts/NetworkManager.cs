using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public int maxPlayers = 10;
    public MainMenu mainMenu;

    public static NetworkManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinOrCreateRoom()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)maxPlayers;

        // TODO look more into matchmaking using Photon
        PhotonNetwork.JoinRandomOrCreateRoom(null, (byte) maxPlayers, MatchmakingMode.FillRoom, null, null, null, options);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void ChangeNickname(string nickname)
    {
        PhotonNetwork.NickName = nickname;
    }

    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("Successfully connected to master server");
        JoinOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        mainMenu.OnJoinedRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ChangeScene("menu_scene");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // TODO reduce alive players
        // Check win conditions
    }
    #endregion
}
