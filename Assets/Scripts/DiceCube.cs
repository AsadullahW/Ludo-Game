using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCube : MonoBehaviour {

    public int value = 6;
    public int iterations = 10;
    public float iterationDelay = 0.1f;
    public Transform[] faces;
    public bool isRolling;
    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    public IEnumerator Roll(int z)
    {
        int randomValue = value;
        isRolling = true;
        gm.waitingForRoll = false;
        for (int i = 0; i < iterations; i++)
        {
            while (randomValue == value)
                randomValue = Random.Range(1, 7);
            value = Random.Range(1, 7);
            for (int j = 0; j < 6; j++)
            {
                if(j + 1 == value)
                {
                    faces[j].gameObject.SetActive(true); 
                }
                else
                {
                    faces[j].gameObject.SetActive(false);
                }
            }
            yield return new WaitForSeconds(iterationDelay);
            value = z;
            for (int j = 0; j < 6; j++)
            {
                if (j + 1 == value)
                {
                    faces[j].gameObject.SetActive(true);
                }
                else
                {
                    faces[j].gameObject.SetActive(false);
                }
            }
            yield return new WaitForSeconds(iterationDelay);
        }
        isRolling = false;
        gm.BeginTurn();
    }
    [PunRPC]
    public void RPC_Roll_Dice(int i)
    {
        StartCoroutine(Roll(i));
    }
}


   