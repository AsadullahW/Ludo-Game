using Photon.Pun;
using UnityEngine;

public class Network_Manager : MonoBehaviourPunCallbacks
{
    public LudoLevel Player_Level;
    private const string LEVEL = "level";
    private const string TEAM = "";
    public const int MAx_Players = 4;
    public static Network_Manager Instance;
    public int Active = 0;

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
    }
    
    private void Start()
    {
        OnConnect_Mine();
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
        Debug.LogError(PhotonNetwork.LocalPlayer.ActorNumber + "No_of_Players_ In_MultiPlayer");
        Prepare_team_selection_options();
    //    Prepare_turn_selection_options();
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.LogError(newPlayer.ActorNumber + "Seconds_Players_ In_MultiPlayer");
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
        
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
              PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { TEAM, team } });
        }        else if(PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
              PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { TEAM, team } });
        }
        else if  (PhotonNetwork.LocalPlayer.ActorNumber == 3)
        {
              PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { TEAM, team } });
        }
        else if(PhotonNetwork.LocalPlayer.ActorNumber == 4)
        {
              PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { TEAM, team } });
        }

       //  PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        
    }
    public void Prepare_team_selection_options()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            Debug.LogError(PhotonNetwork.CurrentRoom.PlayerCount);
            Generic_UI.Instance.Player_no.text = "Player_no : " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();

           var firstPlayer = PhotonNetwork.CurrentRoom.GetPlayer(1);
          
           if(firstPlayer.CustomProperties.ContainsKey(TEAM))
           {
               var occupiedTam = firstPlayer.CustomProperties[TEAM];
               MainMenuUI.Instance.Restricted_Team_Choice((TeamColor)occupiedTam);
           }
        }
        else
        {
            Generic_UI.Instance.Player_no.text = "Player_no : 1";
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
                Debug.LogError("here");
                ShareValues.Current_no++;
                if (ShareValues.Current_no > GameManager.NumberOfPlayers)
                {
                    ShareValues.Current_no = 0;
                }
            }

            //var Current_Player = PhotonNetwork.CurrentRoom.GetPlayer((int)(TeamColor)occupiedTam);

        }
        // var Current_Player = PhotonNetwork.CurrentRoom.GetPlayer((int)(TeamColor)occupiedTam);

    }
}  






   