using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisable : MonoBehaviour
{
    public PhotonView view = null;
    public float time = 3;
    void Start()
    {
        view.RPC(nameof(DisableObj), RpcTarget.AllBuffered);
    }
    [PunRPC]
    void DisableObj()
    {
        Invoke(nameof(OFFObject), time);
    }

    void OFFObject()
    {
        gameObject.SetActive(false);
    }
}
