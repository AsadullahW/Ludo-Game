using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RollDiceButton : MonoBehaviour 
{
    public Animator animator;
    public bool isInteractive;
    public PlayerType playerType;
    public DiceCube diceCube;
    private GameManager gm;
    private const string TEAM = "";
    private PhotonView view;

    public bool arrowShow;
    private void Start()
    {
        gm = GameManager.instance;
        view = GetComponent<PhotonView>();
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
        view.RPC(nameof(OnMouseDownExtract), RpcTarget.All);
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
                    if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && ShareValues.Color_No == 1 && view.IsMine)
                    {
                        transform.position = transform.position - Vector3.up * 0.000003f;

                    }
                    break;
            }
        }
    }
   
    private void OnMouseUp()
    {
        OnMouseUp_Extract();
    }
    //[PunRPC]
    private void OnMouseUp_Extract()
    {
        if (isInteractive == true)
        {
            transform.position = transform.position + Vector3.up * 0.000003f;

            switch (playerType)
            {
                case PlayerType.BLUE:

                    if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    {
                        view.RPC(nameof(Roll_Dice), RpcTarget.AllBuffered, Random.Range(1, 7));
                    }
                    break;

                case PlayerType.GREEN:

                    int nextTurn = 0;
                    if (GameManager.is_2vs2)
                    {
                        nextTurn = 2;
                    }
                    else
                    {
                        nextTurn = 3;
                    }

                    if (PhotonNetwork.LocalPlayer.ActorNumber == nextTurn)
                    {
                        view.RPC(nameof(Roll_Dice), RpcTarget.AllBuffered, Random.Range(1, 7));
                    }
                    break;


                case PlayerType.RED:

                    if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
                    {
                        view.RPC(nameof(Roll_Dice), RpcTarget.AllBuffered, Random.Range(1, 7));
                    }
                    break;

                case PlayerType.YELLOW:

                    if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
                    {
                        view.RPC(nameof(Roll_Dice), RpcTarget.AllBuffered, Random.Range(1, 7));
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