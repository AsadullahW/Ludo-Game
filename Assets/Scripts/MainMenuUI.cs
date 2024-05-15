using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum TeamColor
{
    Blue = 1, Green = 2, Red = 3, Yellow = 4
}
public class MainMenuUI : MonoBehaviour 
{
    public static MainMenuUI Instance;
    public ScreenFader fader;
    public GameObject mainMenu;
    public GameObject Selection_Menu;
    public GameObject loadingPanel;
    public GameObject playMenu;
    public GameObject playerSelectionPanel;
    public GameObject selectedAiPlayerPanel;
    public Dropdown Game_Level_Selection;
    public Text Waiting_Text;

    public Text WaitForOther;

    public int totalPlayer = 0;
    public int roomCapcity = 0;

    public bool _2v2 = false;
    public bool _3v3 = false;

   
    private void Awake()
    {
        Game_Level_Selection.AddOptions(Enum.GetNames(typeof(LudoLevel)).ToList());
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        totalPlayer = 0;
        PhotonNetwork.Disconnect();
    }
    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 4.4f);
    }
    public void Play()
    {
        mainMenu.SetActive(false);
        Selection_Menu.SetActive(true);
    }
    public void JoinMultiplayerLobby()
    {
        Selection_Menu.SetActive(false);
        playerSelectionPanel.SetActive(true);
    }
    public void OnClick2v2Player()
    {
        _2v2 = true;
        GameManager.is_2vs2 = true;
        totalPlayer = 2;
        Network_Manager.Instance.OnConnect_Mine();
        SetMatchAccordingToPlayers();
    }
    public void OnCLick3v3Players()
    {
        _3v3 = true;
        totalPlayer = 3;
        Network_Manager.Instance.OnConnect_Mine();
        SetMatchAccordingToPlayers();
    }
    public void OnClick4v4Players()
    {
        totalPlayer = 4;
        Network_Manager.Instance.OnConnect_Mine();
        SetMatchAccordingToPlayers();
    }
    public void SetMatchAccordingToPlayers()
    {
        playerSelectionPanel.SetActive(false);
        loadingPanel.SetActive(true);
        roomCapcity = totalPlayer;
        GameManager.NumberOfPlayers = totalPlayer;
        Generic_UI.Instance.player_No.gameObject.SetActive(true);
    }
    public void UpdatePlayerInfo()
    {
        totalPlayer -=1;
        if(Waiting_Text != null)
        WaitForOther.text = "Please wait for other " + totalPlayer + " players...";
        if(totalPlayer == 0)
        {
            WaitForOther.text = "Enter in match...";
            StartMatchRoom();
        }
    }

    public void Play_MultiPlayer()
    {
        Selection_Menu.SetActive(false);
        playMenu.SetActive(true);
    }

   
    public void Exit()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    public void Play2P()
    {
        GameManager.is_2vs2 = true;
        GameManager.NumberOfPlayers = 2;
       //Select_team(1);
        fader.FadeTo("MainScene");
    }

    public void PlayWithAI()
    {
        Selection_Menu.SetActive(false);
        selectedAiPlayerPanel.SetActive(true);
    }
    public void OnClick2vs2AiPlayers()
    {
        LoadAiMatch(2);
    }
    public void OnClick3vs3AiPlayers()
    {
        LoadAiMatch(3);
    }
    public void OnClick4vs4AiPlayers()
    {
        LoadAiMatch(4);
    }
    void LoadAiMatch(int totalPlayer)
    {
        fader.FadeTo("LudoAIScene");
        ShareValues.TotalAiPlayers = totalPlayer;
    }
    public void Play3P()
    {
        GameManager.NumberOfPlayers = 3;
        fader.FadeTo("MainScene");
    }

    public void Play4P()
    {
        Generic_UI.Instance.player_No.gameObject.SetActive(true);
        GameManager.NumberOfPlayers = 4;
        fader.FadeTo("MainScene");
    }
    void StartMatchRoom()
    {
        Generic_UI.Instance.player_No.gameObject.SetActive(true);
        fader.FadeTo("MainScene");
    }

    public void OnClickBack()
    {
        Selection_Menu.SetActive(true);
        playerSelectionPanel.SetActive(false);
    }
    public void OnConnect()
    {
        Network_Manager.Instance.Set_Player_Level((LudoLevel)Game_Level_Selection.value);
    }

    public void Select_team(int team)
    { 
        Network_Manager.Instance.select_team(team);
    }
    public void Restricted_Team_Choice(TeamColor occpiedTeam)
    {
        // Button buttonToDeactivate = occpiedTeam == TeamColor.White ? whiteTeamButtonButton : blackTeamButtonButton;
        // buttonToDeactivate.interactable = false;

        if (occpiedTeam == TeamColor.Blue)
        {
            ShareValues.Color_No = 1;

        }
        else if (occpiedTeam == TeamColor.Green)
        {
            ShareValues.Color_No = 3;
        }
        else if (occpiedTeam == TeamColor.Red)
        {
            ShareValues.Color_No = 2;
        }
        else if (occpiedTeam == TeamColor.Yellow)
        {
            ShareValues.Color_No = 4;
        }
    }
}


   