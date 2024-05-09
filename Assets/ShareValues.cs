using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShareValues
{
    public static int Current_no = 1;
    public static int Color_No = 1;
    public static int For_Node_no = 1;

    private static int totalPlayer =0; 

   
    public static int TotalAiPlayers
    {
        get
        {
            return totalPlayer;
        }
        set
        {
            totalPlayer = value;
        }
    }
}


   