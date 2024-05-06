using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisable : MonoBehaviour
{
    public float time = 3;
    void Start()
    {
        Invoke("DisableObj", time);
    }
    void DisableObj()
    {
        gameObject.SetActive(false);
    }
}
