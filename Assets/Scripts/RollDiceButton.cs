using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RollDiceButton : MonoBehaviour {

    public Animator animator;
    public bool isInteractive;
    public PlayerType playerType;
    public DiceCube diceCube;
    private GameManager gm;
    PhotonView View;
    private const string TEAM = "";

    public bool arrowShow;
    private void Start()
    {
        gm = GameManager.instance;
        View = GetComponent<PhotonView>();
    }

    private void Update()
    {
        isInteractive = gm.currentPlayer.playerType == playerType && gm.waitingForRoll == true && !diceCube.isRolling;
        animator.SetBool("isInteractive", isInteractive);
        if (isInteractive == true && arrowShow == false)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            arrowShow = true;
        }
        else if (isInteractive == false && arrowShow == true)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            arrowShow = false;
        }
    }

    private void OnMouseDown()
    {
        View.RPC(nameof(OnMouseDownExtract), RpcTarget.All);
        //OnMouseDownExtract();
    }
    [PunRPC]
    private void OnMouseDownExtract()
    {
        if (isInteractive == true)
        {
            switch (playerType)
            {
                case PlayerType.BLUE:

                    var occupiedTam = 0;
                    MainMenuUI.Instance.Restricted_Team_Choice((TeamColor)occupiedTam);
                    if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && ShareValues.Color_No == 1 && View.IsMine)
                    {
                        transform.position = transform.position - Vector3.up * 0.000003f;

                    }
                    break;
            }
        }
    }
   
    private void OnMouseUp()
    {
        //View.RPC(nameof(OnMouseUp_Extract), RpcTarget.All );
        OnMouseUp_Extract();
    }
    //[PunRPC]
    private void OnMouseUp_Extract()
    {
        if (isInteractive == true  )
        {
            transform.position = transform.position + Vector3.up * 0.000003f;

            switch (playerType)
            {
                case PlayerType.BLUE:
                    if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    {

                     //   Network_Manager.Instance.select_team(1);

                        //ar First_Player =  PhotonNetwork.CurrentRoom.GetPlayer(1);
                        //
                        //  Network_Manager.Instance.select_team(1);
                        //
                        // var occupiedTam = First_Player.CustomProperties[TEAM];
                        // if (((int)(TeamColor)occupiedTam) == 1)
                        // {
                        //     Debug.LogError("Clicked_1");
                        //     View.RPC(nameof(Roll_Dice), RpcTarget.AllBuffered, Random.Range(1, 7));
                        // }

                        View.RPC(nameof(Roll_Dice), RpcTarget.AllBuffered, Random.Range(1, 7));

                    }
                    else 
                    {
                       // ShareValues.Color_No = 5;
                        Debug.LogError("Dont touch");
                    }
                    break;

                case PlayerType.GREEN:


                    if (PhotonNetwork.LocalPlayer.ActorNumber == 3)//2
                    {
                     //   Network_Manager.Instance.select_team(2);

                        //      Network_Manager.Instance.Prepare_turn_selection_options();

                        Debug.LogError("Clicked_3");
                       View.RPC(nameof(Roll_Dice), RpcTarget.AllBuffered, Random.Range(1, 7));

                    }
                    else
                    {
                        Debug.LogError("Dont touch");
                    }
                    break;


                case PlayerType.RED:

                    if (PhotonNetwork.LocalPlayer.ActorNumber == 2)//3
                    {
                        View.RPC(nameof(Roll_Dice), RpcTarget.AllBuffered, Random.Range(1, 7));
                    }
                    else
                    {
                        Debug.LogError("Dont touch");
                    }
                    break;

                case PlayerType.YELLOW:
                    if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
                    {
                      //  if (PhotonNetwork.IsMasterClient)
                      //  {
                      //      View.RPC(nameof(Roll_Dice), RpcTarget.AllBuffered);
                      //  }
                        View.RPC(nameof(Roll_Dice), RpcTarget.AllBuffered , Random.Range(1,7));
                    }
                    else
                    {
                        Debug.LogError("Dont touch");
                    }
                    break;
            }
        }
    }

    [PunRPC]
    public void Roll_Dice(int i)
    {
        diceCube.RPC_Roll_Dice(i);
    }
}