using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.bhambhoo.fairludo;

public class AINode : MonoBehaviour
{

    public List<PlayerToken> allPlayers,redPlayers, yellowPlayers, greenPlayers, bluePlayers;
    
    public List<int> playerIndexeres;
    public int allPlayersCount, bluePlayersCount, redPlayersCount, yellowPlayersCount, greenPlayersCount;
    public bool isSafeNode;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (isSafeNode)
        {
            allPlayers.Add(other.gameObject.GetComponent<PlayerToken>());
            CheckAllPlayersList();
        }
        else
        {
            
            switch (other.gameObject.GetComponent<PlayerToken>().player.playerIndex)
            {
                case 1:bluePlayers.Add(other.gameObject.GetComponent<PlayerToken>());
                    CheckBluePlayersList();

                    return;


                case 2:redPlayers.Add(other.gameObject.GetComponent<PlayerToken>());
                    CheckRedPlayersList();
                    return;

                case 3:greenPlayers.Add(other.gameObject.GetComponent<PlayerToken>());
                    CheckGreenPlayersList();
                    return;

                case 4:yellowPlayers.Add(other.gameObject.GetComponent<PlayerToken>());
                    CheckYellowPlayersList();
                    return;
                default:
                    return;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (isSafeNode)
        {
            
            allPlayers.Remove(other.gameObject.GetComponent<PlayerToken>());
            
            CheckAllPlayersList();
            
        }
        else
        {
            switch (other.gameObject.GetComponent<PlayerToken>().player.playerIndex)
            {
                case 1:
                    bluePlayers.Remove(other.gameObject.GetComponent<PlayerToken>());
                    CheckBluePlayersList();
                    return;

                case 2:
                    redPlayers.Remove(other.gameObject.GetComponent<PlayerToken>());
                    CheckRedPlayersList();
                    return;

                case 3:
                    greenPlayers.Remove(other.gameObject.GetComponent<PlayerToken>());
                    CheckGreenPlayersList();
                    return;

                case 4:
                    yellowPlayers.Remove(other.gameObject.GetComponent<PlayerToken>());
                    CheckYellowPlayersList();
                    return;


                default:
                    return;
            }
        }
    }

    public void CheckAllPlayersList()
    {
        if (allPlayers.Count == 1)
        {
             allPlayers[0].transform.position = this.transform.position;
            allPlayers[0].transform.localScale = new Vector3(3.3f,3.3f, 3.3f);
            Debug.LogError("the Player Pos is : " + allPlayers[0].transform.position);

            return;
        }
        if (allPlayers.Count == 2)
        {
           
            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            allPlayers[0].transform.localScale = Vector3.Slerp(allPlayers[0].transform.localScale, new Vector3(2.2f, 2.2f, 2.2f), .5f); ;
            allPlayers[0].transform.position = firstCubePosition;

            allPlayers[1].transform.localScale = Vector3.Slerp(allPlayers[1].transform.localScale, new Vector3(2.2f, 2.2f, 2.2f), .5f); ;
            allPlayers[1].transform.position = secondCubePosition;

            return;
        }
        if (allPlayers.Count == 3)
        {

            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            Vector3 thiredCubePosition = this.transform.position + new Vector3(0f, 0f, 1f);

            allPlayers[0].transform.localScale = Vector3.Slerp(allPlayers[0].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            allPlayers[0].transform.position = firstCubePosition;

            allPlayers[1].transform.localScale = Vector3.Slerp(allPlayers[1].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            allPlayers[1].transform.position = secondCubePosition;

            allPlayers[2].transform.localScale = Vector3.Slerp(allPlayers[2].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            allPlayers[2].transform.position = thiredCubePosition;

            return;
        }
        if (allPlayers.Count == 4)
        {

            Vector3 firstCubePosition = this.transform.position + new Vector3(-1.8f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1.8f, 0f, 0f);

            Vector3 thiredCubePosition = this.transform.position + new Vector3(0f, 0f, 1.8f);

            Vector3 fouthCubePosition = this.transform.position + new Vector3(0f, 0f, -1.8f);

            allPlayers[0].transform.localScale = Vector3.Slerp(allPlayers[0].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            allPlayers[0].transform.position = firstCubePosition;

            allPlayers[1].transform.localScale = Vector3.Slerp(allPlayers[1].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            allPlayers[1].transform.position = secondCubePosition;

            allPlayers[2].transform.localScale = Vector3.Slerp(allPlayers[2].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            allPlayers[2].transform.position = thiredCubePosition;

            allPlayers[3].transform.localScale = Vector3.Slerp(allPlayers[3].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            allPlayers[3].transform.position = fouthCubePosition;


            return;
        }
    }

    public void CheckBluePlayersList()
    {
        if (bluePlayers.Count == 1)
        {
            
            bluePlayers[0].transform.position = this.transform.position;
            bluePlayers[0].transform.localScale = new Vector3(3.3f, 3.3f, 3.3f);
           
            return;
        }
        if (bluePlayers.Count == 2)
        {
           
            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            bluePlayers[0].transform.localScale = Vector3.Slerp(bluePlayers[0].transform.localScale, new Vector3(2.2f, 2.2f, 2.2f), .5f); ;
            bluePlayers[0].transform.position = firstCubePosition;

            bluePlayers[1].transform.localScale = Vector3.Slerp(bluePlayers[1].transform.localScale, new Vector3(2.2f, 2.2f, 2.2f), .5f); ;
            bluePlayers[1].transform.position = secondCubePosition;

            return;
        }
        if (bluePlayers.Count == 3)
        {
            
            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            Vector3 thiredCubePosition = this.transform.position + new Vector3(0f, 0f, 1f);

            bluePlayers[0].transform.localScale = Vector3.Slerp(bluePlayers[0].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            bluePlayers[0].transform.position = firstCubePosition;
            
            bluePlayers[1].transform.localScale = Vector3.Slerp(bluePlayers[1].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            bluePlayers[1].transform.position = secondCubePosition;
            
            bluePlayers[2].transform.localScale = Vector3.Slerp(bluePlayers[2].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            bluePlayers[2].transform.position = thiredCubePosition;

            return;
        }
        if (bluePlayers.Count == 4)
        {
            
            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            Vector3 thiredCubePosition = this.transform.position + new Vector3(0f, 0f, 1f);

            Vector3 fouthCubePosition = this.transform.position + new Vector3(0f, 0f, -1f);

            bluePlayers[0].transform.localScale = Vector3.Slerp(bluePlayers[0].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            bluePlayers[0].transform.position = firstCubePosition;
            
            bluePlayers[1].transform.localScale = Vector3.Slerp(bluePlayers[1].transform.localScale, new Vector3(1, 1f, 1f), .5f); ;
            bluePlayers[1].transform.position = secondCubePosition;
            
            bluePlayers[2].transform.localScale = Vector3.Slerp(bluePlayers[2].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            bluePlayers[2].transform.position = thiredCubePosition;
            
            bluePlayers[3].transform.localScale = Vector3.Slerp(bluePlayers[3].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            bluePlayers[3].transform.position = fouthCubePosition;


            return;
        }
    }

    public void CheckRedPlayersList()
    {
        if (redPlayers.Count == 1)
        {
            
            redPlayers[0].transform.position = this.transform.position;
            redPlayers[0].transform.localScale = new Vector3(3.3f, 3.3f, 3.3f);
            
            return;
        }
        if (redPlayers.Count == 2)
        {
            
            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            redPlayers[0].transform.localScale = Vector3.Slerp(redPlayers[0].transform.localScale, new Vector3(2.2f, 2.2f, 2.2f), .5f); ;
            redPlayers[0].transform.position = firstCubePosition;

            redPlayers[1].transform.localScale = Vector3.Slerp(redPlayers[1].transform.localScale, new Vector3(2.2f, 2.2f, 2.2f), .5f); ;
            redPlayers[1].transform.position = secondCubePosition;

            return;
        }
        if (redPlayers.Count == 3)
        {
            
            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            Vector3 thiredCubePosition = this.transform.position + new Vector3(0f, 0f, 1f);

            redPlayers[0].transform.localScale = Vector3.Slerp(redPlayers[0].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            redPlayers[0].transform.position = firstCubePosition;

            redPlayers[1].transform.localScale = Vector3.Slerp(redPlayers[1].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            redPlayers[1].transform.position = secondCubePosition;

            redPlayers[2].transform.localScale = Vector3.Slerp(redPlayers[2].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            redPlayers[2].transform.position = thiredCubePosition;

            return;
        }
        if (redPlayers.Count == 4)
        {
            
            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            Vector3 thiredCubePosition = this.transform.position + new Vector3(0f, 0f, 1f);

            Vector3 fouthCubePosition = this.transform.position + new Vector3(0f, 0f, -1f);

            redPlayers[0].transform.localScale = Vector3.Slerp(redPlayers[0].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            redPlayers[0].transform.position = firstCubePosition;

            redPlayers[1].transform.localScale = Vector3.Slerp(redPlayers[1].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            redPlayers[1].transform.position = secondCubePosition;

            redPlayers[2].transform.localScale = Vector3.Slerp(redPlayers[2].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            redPlayers[2].transform.position = thiredCubePosition;

            redPlayers[3].transform.localScale = Vector3.Slerp(redPlayers[3].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            redPlayers[3].transform.position = fouthCubePosition;


            return;
        }
    }

    public void CheckGreenPlayersList()
    {
        if (greenPlayers.Count == 1)
        {
            
            greenPlayers[0].transform.position = this.transform.position;
            greenPlayers[0].transform.localScale = new Vector3(3.3f, 3.3f, 3.3f);
            
            return;
        }
        if (greenPlayers.Count == 2)
        {
            
            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            greenPlayers[0].transform.localScale = Vector3.Slerp(greenPlayers[0].transform.localScale, new Vector3(2.2f, 2.2f, 2.2f), .5f); ;
            greenPlayers[0].transform.position = firstCubePosition;

            greenPlayers[1].transform.localScale = Vector3.Slerp(greenPlayers[1].transform.localScale, new Vector3(2.2f, 2.2f, 2.2f), .5f); ;
            greenPlayers[1].transform.position = secondCubePosition;

            return;
        }
        if (greenPlayers.Count == 3)
        {
            
            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            Vector3 thiredCubePosition = this.transform.position + new Vector3(0f, 0f, 1f);

            greenPlayers[0].transform.localScale = Vector3.Slerp(greenPlayers[0].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            greenPlayers[0].transform.position = firstCubePosition;

            greenPlayers[1].transform.localScale = Vector3.Slerp(greenPlayers[1].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            greenPlayers[1].transform.position = secondCubePosition;

            greenPlayers[2].transform.localScale = Vector3.Slerp(greenPlayers[2].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            greenPlayers[2].transform.position = thiredCubePosition;

            return;
        }
        if (greenPlayers.Count == 4)
        {
            
            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            Vector3 thiredCubePosition = this.transform.position + new Vector3(0f, 0f, 1f);

            Vector3 fouthCubePosition = this.transform.position + new Vector3(0f, 0f, -1f);

            greenPlayers[0].transform.localScale = Vector3.Slerp(greenPlayers[0].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            greenPlayers[0].transform.position = firstCubePosition;

            greenPlayers[1].transform.localScale = Vector3.Slerp(greenPlayers[1].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            greenPlayers[1].transform.position = secondCubePosition;

            greenPlayers[2].transform.localScale = Vector3.Slerp(greenPlayers[2].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            greenPlayers[2].transform.position = thiredCubePosition;

            greenPlayers[3].transform.localScale = Vector3.Slerp(greenPlayers[3].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            greenPlayers[3].transform.position = fouthCubePosition;


            return;
        }
    }

    public void CheckYellowPlayersList()
    {
        if (yellowPlayers.Count == 1)
        {

            yellowPlayers[0].transform.position = this.transform.position;
            yellowPlayers[0].transform.localScale = new Vector3(3.3f, 3.3f, 3.3f);
            
            return;
        }
        if (yellowPlayers.Count == 2)
        {
            
            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            yellowPlayers[0].transform.localScale = Vector3.Slerp(yellowPlayers[0].transform.localScale, new Vector3(2.2f, 2.2f, 2.2f), .5f);
            yellowPlayers[0].transform.position = firstCubePosition;

            yellowPlayers[1].transform.localScale = Vector3.Slerp(yellowPlayers[1].transform.localScale, new Vector3(2.2f, 2.2f, 2.2f), .5f);
            yellowPlayers[1].transform.position = secondCubePosition;

            return;
        }
        if (yellowPlayers.Count == 3)
        {
            
            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            Vector3 thiredCubePosition = this.transform.position + new Vector3(0f, 0f, 1f);

            yellowPlayers[0].transform.localScale = Vector3.Slerp(yellowPlayers[0].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            yellowPlayers[0].transform.position = firstCubePosition;

            yellowPlayers[1].transform.localScale = Vector3.Slerp(yellowPlayers[1].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            yellowPlayers[1].transform.position = secondCubePosition;

            yellowPlayers[2].transform.localScale = Vector3.Slerp(yellowPlayers[2].transform.localScale, new Vector3(1.7f, 1.7f, 1.7f), .5f); ;
            yellowPlayers[2].transform.position = thiredCubePosition;

            return;
        }
        if (yellowPlayers.Count == 4)
        {
            
            Vector3 firstCubePosition = this.transform.position + new Vector3(-1f, 0f, 0f);

            Vector3 secondCubePosition = this.transform.position + new Vector3(1f, 0f, 0f);

            Vector3 thiredCubePosition = this.transform.position + new Vector3(0f, 0f, 1f);

            Vector3 fouthCubePosition = this.transform.position + new Vector3(0f, 0f, -1f);

            yellowPlayers[0].transform.localScale = Vector3.Slerp(yellowPlayers[0].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            yellowPlayers[0].transform.position = firstCubePosition;

            yellowPlayers[1].transform.localScale = Vector3.Slerp(yellowPlayers[1].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            yellowPlayers[1].transform.position = secondCubePosition;

            yellowPlayers[2].transform.localScale = Vector3.Slerp(yellowPlayers[2].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            yellowPlayers[2].transform.position = thiredCubePosition;

            yellowPlayers[3].transform.localScale = Vector3.Slerp(yellowPlayers[3].transform.localScale, new Vector3(1f, 1f, 1f), .5f); ;
            yellowPlayers[3].transform.position = fouthCubePosition;


            return;
        }
    }
}
