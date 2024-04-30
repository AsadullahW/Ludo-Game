using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Photon.Pun.UtilityScripts.PunTeams;

public enum TeamColor
{
    Blue = 1, Green = 2, Red = 3, Yellow = 4
}
public class MainMenuUI : MonoBehaviour {

    public static MainMenuUI Instance;
    public ScreenFader fader;
    public GameObject mainMenu;
    public GameObject Selection_Menu;
    public GameObject playMenu;
    public Dropdown Game_Level_Selection;
    public Text Waiting_Text;

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
    }
    private void Start()
    {

    }
    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 4.4f);
    }
    public void Play()
    {
        if(Generic_UI.Instance.Con_State.text == "Joined")
        {
            mainMenu.SetActive(false);
            Selection_Menu.SetActive(true);
        }

    }
    public void Play_Single_Player()
    {
        if(Generic_UI.Instance.Con_State.text == "Joined")
        {
            //Selection_Menu.SetActive(false);
            //playMenu.SetActive(true);
            Play2P();
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
        GameManager.NumberOfPlayers = 2;
       //Select_team(1);
        fader.FadeTo("MainScene");
    }

    public void PlayWithAI()
    {
        fader.FadeTo("LudoAIScene");
    }

    public void Play3P()
    {
        GameManager.NumberOfPlayers = 3;
        fader.FadeTo("MainScene");
    }

    public void Play4P()
    {
        GameManager.NumberOfPlayers = 4;
        fader.FadeTo("MainScene");
    }

    public void Back()
    {
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
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
       if(occpiedTeam == TeamColor.Blue)
       {
            ShareValues.Color_No = 1;
            
       }
       else if (occpiedTeam == TeamColor.Green)
        {
            ShareValues.Color_No = 2;
        }
        else if (occpiedTeam == TeamColor.Red)
        {
            ShareValues.Color_No = 3;
        }
        else if (occpiedTeam == TeamColor.Yellow)
        {
            ShareValues.Color_No = 4;
        }
    }

}


   