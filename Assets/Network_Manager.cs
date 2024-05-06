using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class Network_Manager : MonoBehaviourPunCallbacks
{
    public LudoLevel Player_Level;
    private const string LEVEL = "level";
    private const string TEAM = "";
    public const int MAx_Players = 4;
    public static Network_Manager Instance;
    public int Active = 0;

    PhotonView view;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        PhotonNetwork.AutomaticallySyncScene = true;
        view = GetComponent<PhotonView>();
    }
    public void OnConnect_Mine()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable() { { LEVEL, Player_Level } }, MAx_Players);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
   
    private void Update()
    {
        Generic_UI.Instance.Con_State.text =  PhotonNetwork.NetworkClientState.ToString();
        if (Generic_UI.Instance.Con_State.text == "Joined")
        {
            if(MainMenuUI.Instance.Waiting_Text)
            {
                MainMenuUI.Instance.Waiting_Text.gameObject.SetActive(false);
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable() { { LEVEL, Player_Level } }, MAx_Players);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null , new Photon.Realtime.RoomOptions
        {
            CustomRoomPropertiesForLobby = new string[] {LEVEL},
            MaxPlayers = MAx_Players,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { LEVEL, Player_Level } },
        });
    }
    public override void OnJoinedRoom()
    {
        view.RPC(nameof(WaitForOtherPlayer), RpcTarget.AllBuffered);
        Prepare_team_selection_options();
    }

    [PunRPC]
    public void WaitForOtherPlayer()
    {
        MainMenuUI.Instance.UpdatePlayerInfo();
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {

    }

    public void Set_Player_Level(LudoLevel level)
    {
        Player_Level = level;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { LEVEL, level } });
    }
    public void set_team()
    {
        int team = 0;
        while (PhotonNetwork.LocalPlayer.ActorNumber < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { TEAM, team } });
        }
    }   
    public void select_team(int team)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            Debug.Log("Set the cutom properties player" + PhotonNetwork.LocalPlayer.ActorNumber);
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { TEAM, team } });
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            Debug.Log("Set the cutom properties player" + PhotonNetwork.LocalPlayer.ActorNumber);
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { TEAM, team } });
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
        {
            Debug.Log("Set the cutom properties player" + PhotonNetwork.LocalPlayer.ActorNumber);
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { TEAM, team } });
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
        {
            Debug.Log("Set the cutom properties player" + PhotonNetwork.LocalPlayer.ActorNumber);
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { TEAM, team } });
        }

        //  PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
    }
    public void Prepare_team_selection_options()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
            Generic_UI.Instance.playerLeft_Info.text = "Player_no : " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();

           var firstPlayer = PhotonNetwork.CurrentRoom.GetPlayer(1);
          
           if(firstPlayer.CustomProperties.ContainsKey(TEAM))
           {
               var occupiedTam = firstPlayer.CustomProperties[TEAM];
               MainMenuUI.Instance.Restricted_Team_Choice((TeamColor)occupiedTam);
           }
        }
        else
        {
            Generic_UI.Instance.playerLeft_Info.text = "Player_no : 1";
        }
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
       
        if (otherPlayer == PhotonNetwork.LocalPlayer)
        {
            Debug.Log("You left the match.");
        }
        else
        {
            Debug.Log("Player " + otherPlayer.ActorNumber + " left the match.");
            photonView.RPC("PlayerLeftMessage", RpcTarget.All, otherPlayer.ActorNumber);
        }
    }

    [PunRPC]
    void PlayerLeftMessage(string playerName)
    {
        Debug.Log(playerName + " left the match.");
        Generic_UI.Instance.playerLeft_Info.text = "Player " + playerName + " left the match.";
        Generic_UI.Instance.playerLeft_Info.gameObject.SetActive(true);
    }
    public void Prepare_turn_selection_options()
    {

       // set_team();
        var Current_Player = PhotonNetwork.CurrentRoom.GetPlayer(ShareValues.Current_no);

        if (Current_Player.CustomProperties.ContainsKey(TEAM))
        {
            var occupiedTam = Current_Player.CustomProperties[TEAM];
            if (((int)(TeamColor)occupiedTam) == 1)
            {
                PhotonNetwork.SetMasterClient(Current_Player);
                ShareValues.Current_no++;
                if (ShareValues.Current_no > GameManager.NumberOfPlayers)
                {
                    ShareValues.Current_no = 0;
                }
            }
        }
    }
} 