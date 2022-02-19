using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject roomScreen;

    public TMP_Text playerList;
    public TMP_Text roomInfo;

    #region Main Screen
    public void OnPlayerName_ValueChanged(TMP_InputField playerName)
    {
        NetworkManager.instance.ChangeNickname(playerName.text);
    }

    public void OnSplitScreen_Click()
    {
        // TODO Implement split screen lobby and joining
    }

    public void OnJoinGame_Click()
    {
        NetworkManager.instance.ConnectToMaster();
    }

    public void OnExitGame_Click()
    {
        Application.Quit();
    }
    #endregion

    #region Room Screen
    public void OnJoinedRoom()
    {
        mainScreen.SetActive(false);
        roomScreen.SetActive(true);

        gameObject.GetPhotonView().RPC("UpdateRoomUI", RpcTarget.All);
    }

    [PunRPC]
    public void UpdateRoomUI()
    {
        playerList.text = "";
        roomInfo.text = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerList.text += player.NickName + "\n";
        }

        roomInfo.text = $"<b>Players:</b> {PhotonNetwork.CurrentRoom.PlayerCount}/{NetworkManager.instance.maxPlayers}\n";
        // TODO add a countdown
    }

    public void OnLeaveRoom_Click()
    {
        NetworkManager.instance.LeaveRoom();
        roomScreen.SetActive(false);
        mainScreen.SetActive(true);
    }

    public void OnStartGame_Click()
    {
        NetworkManager.instance.JoinOrCreateRoom();
    }
    #endregion
}
