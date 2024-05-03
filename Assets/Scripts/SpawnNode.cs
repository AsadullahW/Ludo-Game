using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNode : MonoBehaviour {

    public Transform nextNode;
    public Token token;
    public bool interactable = false;

    public float smoothness = 10f;
    PhotonView View;


    private void Start()
    {
        View = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (token == null)
            return;
        if (token.tokenStatus == TokenStatus.LOCKED_IN_SPAWN)
        {
            token.tokenTransform.position = Vector3.Slerp(token.tokenTransform.position, GetPosition(), smoothness * Time.deltaTime);
        }
    }

    public Vector3 offset = new Vector3(0, 3, 0);

    public Vector3 GetPosition()
    {
        return transform.position + offset;
    }

    private void OnMouseDown()
    {
        Debug.Log("SpawnNode + On mouseDown");
        //  View.RPC(nameof(Interact), RpcTarget.AllBuffered);
        Interact();
    }
    public void Interact()
    {
        Interact_New();
    }

    private void Interact_New()
    {
        if (interactable == true)
        {
            switch (token.tokenType)
            {
                case PlayerType.BLUE:

                    if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
                    {
                        Debug.LogError("Blue");
                        View.RPC(nameof(New_One), RpcTarget.AllBuffered);
                    }
                    else
                    {
                        ShareValues.Color_No = 5;
                        Debug.LogError("Dont touch");
                    }
                    break;

                case PlayerType.GREEN:

                    if (PhotonNetwork.LocalPlayer.ActorNumber == 3)//2
                    {
                        Debug.LogError("Green");
                        View.RPC(nameof(New_One), RpcTarget.AllBuffered);
                    }
                    else
                    {
                        ShareValues.Color_No = 5;
                        Debug.LogError("Dont touch");
                    }
                    break;

                case PlayerType.RED:

                    if (PhotonNetwork.LocalPlayer.ActorNumber == 2)//3
                    {
                        Debug.LogError("Red");
                        View.RPC(nameof(New_One), RpcTarget.AllBuffered);
                    }
                    else
                    {
                        ShareValues.Color_No = 5;
                        Debug.LogError("Dont touch");
                    }
                    break;

                case PlayerType.YELLOW:

                    if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
                    {
                        Debug.LogError("Yellow");
                        View.RPC(nameof(New_One), RpcTarget.AllBuffered);
                    }
                    else
                    {
                        ShareValues.Color_No = 5;
                        Debug.LogError("Dont touch");
                    }
                    break;
            }
            interactable = false;
        }
          //  View.RPC(nameof(GameManager.instance.PlayWithChosenToken_func), RpcTarget.AllBuffered, token);
    }
    [PunRPC]
    public void New_One()
    {
        GameManager.instance.StartCoroutine(GameManager.instance.PlayWithChosenToken(token));
    }
}



   