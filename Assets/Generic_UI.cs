using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generic_UI : MonoBehaviour
{
    public Text Con_State;
    public Text player_No;
    public Text playerLeft_Info;
    public static Generic_UI Instance;
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
    }
}

   