using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Audio;
using Photon.Pun;

public class PauseMenuUI : MonoBehaviour 
{

    public ScreenFader fader;
    public Ground ground;
    public DynamicCamera cam;
    public PostProcessingBehaviour cc;

    public void SetDynamicCam(bool isDynamic)
    {
        cam.isDynamic = isDynamic;
        if (!cam.isDynamic)
        {
            cam.transform.position = cam.offset;
        }
    }

    public void SetCC(bool isEnabled)
    {
        cc.enabled = isEnabled;
    }

    public void SetVolume(float value)
    {
        AudioManager.instance.GetComponent<AudioSource>().volume = value;
    }

    public void Menu()
    {
        if (PhotonNetwork.InRoom)
        {
            
            GameManager.NumberOfPlayers--;
            GameManager.totalPlayerInMatch = GameManager.NumberOfPlayers;

            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();

            PhotonNetwork.AutomaticallySyncScene = false;
            Destroy(Network_Manager.Instance.gameObject, 0.5f);
          
           
        }
        Generic_UI.Instance.player_No.gameObject.SetActive(false);
        Generic_UI.Instance.player_No.text = string.Empty;
        fader.FadeTo("MainMenu");
    }

    public void Retry()
    {
        fader.FadeTo("MainScene");
    }

    public void Rotate()
    {
        ground.SetRotation(Quaternion.Euler((ground.rotation.eulerAngles + new Vector3(0f, 90f, 0f))));
    }

    public void Quit()
    {
        Application.Quit();
    }

}
