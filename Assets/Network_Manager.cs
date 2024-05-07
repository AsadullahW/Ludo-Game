using Photon.Pun;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class Network_Manager : MonoBehaviourPunCallbacks
{
    public LudoLevel Player_Level;
    private const string LEVEL = "level";
    private const string TEAM = "";
    public const int MAx_Players = 4;
    public static Network_Manager Instance;
    public int Active = 0;

    private bool playerRemove = false;

    private PhotonView view;
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
            Generic_UI.Instance.player_No.text = "Player_no : " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();

           var firstPlayer = PhotonNetwork.CurrentRoom.GetPlayer(1);
          
           if(firstPlayer.CustomProperties.ContainsKey(TEAM))
           {
               var occupiedTam = firstPlayer.CustomProperties[TEAM];
               MainMenuUI.Instance.Restricted_Team_Choice((TeamColor)occupiedTam);
           }
        }
        else
        {
            Generic_UI.Instance.player_No.text = "Player_no : 1";
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
            Debug.LogError("Player Left");
            playerRemove = true;
            view.RPC(nameof(PlayerLeftMessage), RpcTarget.AllBuffered, otherPlayer.ActorNumber,otherPlayer.NickName);
        }
    }

    [PunRPC]
    void PlayerLeftMessage(int actorName,string playerName)
    {
        Generic_UI.Instance.playerLeft_Info.gameObject.SetActive(true);
        Generic_UI.Instance.playerLeft_Info.text = "Player " + actorName + " left the match.";

        if (GameManager.instance)
        {
            switch (actorName)
            {
                
                case 1:


                    for (int i = 0; i < GameManager.instance.blueSpawnNodesTransforms.Length; i++)
                    {
                        GameManager.instance.blueSpawnNodesTransforms[i].gameObject.SetActive(false);
                    }

                    GameObject destroyToken = GameManager.instance.blueToken_Holder.gameObject;
                    int loopIteration = GameManager.instance.blueToken_Holder.transform.childCount;

                    for (int i=0;i< loopIteration; i++)
                    {
                        GameObject tokenB = destroyToken.transform.GetChild(i).gameObject;
                        TokenComponent tokenComponentBlue = tokenB.GetComponent<TokenComponent>();

                        if(tokenComponentBlue.tokenInstance.parentNode != null)
                        {
                            Node node = tokenComponentBlue.tokenInstance.parentNode.GetComponent<Node>();

                            node.players.Remove(tokenComponentBlue.tokenInstance);
                            node.bluePlayers.Remove(tokenComponentBlue.tokenInstance);
                            node.allowsKilling = false;
                        }
                        Destroy(tokenB); 
                    }

                    if (playerRemove)
                    {
                        playerRemove = false;
                        IdentifyExitPlayer("Blue");
                    }
                    break;

                case 2:

                    if(GameManager.is_2vs2)
                    {
                        for (int i = 0; i < GameManager.instance.greenSpawnNodesTransforms.Length; i++)
                        {
                            GameManager.instance.greenSpawnNodesTransforms[i].gameObject.SetActive(false);
                        }

                        GameObject token = GameManager.instance.greenToken_Holder.gameObject;
                        int loopScope = GameManager.instance.greenToken_Holder.transform.childCount;

                        for (int i = 0; i < loopScope; i++)
                        {
                            GameObject tokenG = token.transform.GetChild(i).gameObject;
                            TokenComponent tokenComponentGreen = tokenG.GetComponent<TokenComponent>();

                            if (tokenComponentGreen.tokenInstance.parentNode != null)
                            {
                                Node node = tokenComponentGreen.tokenInstance.parentNode.GetComponent<Node>();
                                node.players.Remove(tokenComponentGreen.tokenInstance);
                                node.greenPlayers.Remove(tokenComponentGreen.tokenInstance);
                                node.allowsKilling = false;
                            }
                            Destroy(tokenG);
                        }

                        if (playerRemove)
                        {
                            playerRemove = false;
                            IdentifyExitPlayer("Green");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < GameManager.instance.redSpawnNodesTransforms.Length; i++)
                        {
                            GameManager.instance.redSpawnNodesTransforms[i].gameObject.SetActive(false);
                        }

                        GameObject tokenRed = GameManager.instance.redToken_Holder.gameObject;
                        int loopItration1 = GameManager.instance.redToken_Holder.transform.childCount;

                        for (int i = 0; i < loopItration1; i++)
                        {
                            GameObject tokenR = tokenRed.transform.GetChild(i).gameObject;
                            TokenComponent tokenComponentRed = tokenR.GetComponent<TokenComponent>();

                            if (tokenComponentRed.tokenInstance.parentNode != null)
                            {
                                Node node = tokenComponentRed.tokenInstance.parentNode.GetComponent<Node>();
                                node.players.Remove(tokenComponentRed.tokenInstance);
                                node.redPlayers.Remove(tokenComponentRed.tokenInstance);
                                node.allowsKilling = false;
                            }
                            Destroy(tokenR);
                        }
                        if (playerRemove)
                        {
                            playerRemove = false;
                            IdentifyExitPlayer("Red");
                        }
                    }
                    break;

                case 3:

                    for (int i = 0; i < GameManager.instance.greenSpawnNodesTransforms.Length; i++)
                    {
                        GameManager.instance.greenSpawnNodesTransforms[i].gameObject.SetActive(false);
                    }

                    GameObject tokens = GameManager.instance.greenToken_Holder.gameObject;
                    int loopItration = GameManager.instance.greenToken_Holder.transform.childCount;

                    for (int i = 0; i < loopItration; i++)
                    {
                        GameObject tokenG = tokens.transform.GetChild(i).gameObject;
                        TokenComponent tokenComponentGreen = tokenG.GetComponent<TokenComponent>();

                        if (tokenComponentGreen.tokenInstance.parentNode != null)
                        {
                            Node node = tokenComponentGreen.tokenInstance.parentNode.GetComponent<Node>();
                            node.players.Remove(tokenComponentGreen.tokenInstance);
                            node.greenPlayers.Remove(tokenComponentGreen.tokenInstance);
                            node.allowsKilling = false;
                        }
                        Destroy(tokenG);
                    }
                    if (playerRemove)
                    {
                        playerRemove = false;
                        IdentifyExitPlayer("Green");
                    }
                    break;

                case 4:

                    for (int i = 0; i < GameManager.instance.yellowSpawnNodesTransforms.Length; i++)
                    {
                        GameManager.instance.yellowSpawnNodesTransforms[i].gameObject.SetActive(false);
                    }

                    GameObject token1 = GameManager.instance.yellowToken_Holder.gameObject;
                    int loopItrations = GameManager.instance.yellowToken_Holder.transform.childCount;

                    for (int i = 0; i < loopItrations; i++)
                    {
                        GameObject tokenY = token1.transform.GetChild(i).gameObject;
                        TokenComponent tokenComponentYellow = tokenY.GetComponent<TokenComponent>();

                        if (tokenComponentYellow.tokenInstance.parentNode != null)
                        {
                            Node node = tokenComponentYellow.tokenInstance.parentNode.GetComponent<Node>();
                            node.players.Remove(tokenComponentYellow.tokenInstance);
                            node.yellowPlayers.Remove(tokenComponentYellow.tokenInstance);
                            node.allowsKilling = false;
                        }
                        Destroy(tokenY);

                    }
                    if(playerRemove)
                    {
                        playerRemove = false;

                        IdentifyExitPlayer("Yellow");
                    }
                    break;
            }
        }
    }

    void IdentifyExitPlayer(string playerName)
    {
        for (int i = 0; i < GameManager.instance.players.Count; i++)
        {
            if (GameManager.instance.players[i].name.Equals(playerName))
            {
                view.RPC(nameof(RemovePlayerByName), RpcTarget.AllBuffered, GameManager.instance.players[i].name);
                break;
            }
        }

        GameManager.instance.WinningMessageShow();
    }
    [PunRPC]
    public void RemovePlayerByName(string playerName)
    {
        Player playerToRemove = GameManager.instance.players.Find(player => player.name == playerName);
        if (playerToRemove != null)
        {
            GameManager.instance.players.Remove(playerToRemove);
            view.RPC(nameof(RemovePlayerFromMatch), RpcTarget.AllBuffered, playerName);
        }
    }
    [PunRPC]
    void RemovePlayerFromMatch(string playerName)
    {
        Player playerToRemove = GameManager.instance.players.Find(player => player.name == playerName);
        if (playerToRemove != null)
        {
            GameManager.instance.players.Remove(playerToRemove);
        }
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