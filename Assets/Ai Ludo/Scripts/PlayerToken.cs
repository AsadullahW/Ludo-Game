using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace com.bhambhoo.fairludo
{
    public class PlayerToken : MonoBehaviour
    {
        static int[] safeWaypoints = { 0, 8, 13, 21, 26, 34, 39, 47 };
        // From 1 to 4
        //public int playerIndex = 1;
        // Also from 1 to 4. This is used to move particular token on other clients.
        public int tokenIndex = 1;
        // Waypoint index according to this player's path. -1 means player is at base.
        public int localWaypointIndex = -1;

        public bool tokenPassed;

        public GameObject InputHighlight;

        // This token will always return to this base when it dies.
        public Transform Base;

        private void OnMouseDown()
        {
            MatchManager.Instance.OnTokenTouchUserInput(this);
            // Match manager will take care if this token touch was being awaited or not.
        }

        public void Highlight(bool on)
        {
            InputHighlight.SetActive(on);
        }

        public PlayerAI player;

        /// <summary>
        /// This method is used to initialize this token.
        /// </summary>
        /// <param name="PlayerIndex">Any from 1 to 4.</param>
        /// <param name="Base">The player base this should stand upon, in the start.</param>
        public PlayerToken Initialize(PlayerAI player, Transform Base, PlayerToken tokenColor)
        {
            this.Base = Base;
            transform.position = Base.position;
            localWaypointIndex = -1;

            this.player = player;

            //GetComponent<SpriteRenderer>().color = tokenColor;

            return this;
        }

        
        /// <summary>
        /// This function will start moving the token strictly, without any checks.
        /// Checks like if the token IsLocal and if the token CanMove should be made before calling this function.
        /// </summary>
        /// <param name="diceResult"></param>
        public void Move(int diceResult)
        {
            StartCoroutine(MoveCoroutine(diceResult, localWaypointIndex));
        }
        public void MoveNonLocalPlayer(int diceResult, int currentWaypointIndex)
        {
            StartCoroutine(MoveCoroutine(diceResult, currentWaypointIndex));
        }

        IEnumerator MoveCoroutine(int diceResult, int currentWaypointIndex)
        {
            // The remaining Steps is provided to place token at certain position
            // before starting the move, to sync in case of network lag.
            if (!player.IsLocal && localWaypointIndex != currentWaypointIndex)
            {
                // teleport to the correct waypoint
                transform.position = Constants.Instance.GetWaypoint(player.playerIndex, currentWaypointIndex).position;
                localWaypointIndex = currentWaypointIndex;
            }

            if (localWaypointIndex == -1 && diceResult == 6)
                diceResult = 1;

            for (int i = 0; i < diceResult; i++)
            {
                yield return new WaitForSeconds(Constants.delayBetweenTokenMoves);
                localWaypointIndex++;
                SanUtils.PlaySound(Constants.Instance.sfxTokenHop, MatchManager.Instance.audioSource);
                transform.position = Constants.Instance.GetWaypoint(player.playerIndex, localWaypointIndex).position;
            }

            yield return new WaitForSeconds(Constants.delayAfterTokenMoveComplete);

            // Check if we've reached endpoint, or we've killed another token
            if (localWaypointIndex == Constants.LastWaypointIndex)
            {
                Debug.LogError("Player " + player.playerIndex + " reached endpoint, should get an extra turn!");
                //Debug.LogError("Player " + PlayersManager.Players[player.playerIndex] + " reached endpoint, should get an extra turn!");
                Debug.LogError("Player Index before Imp " + player.playerIndex);

                if (player.playerIndex == 3)
                {
                   
                        AIGameManager.Instance.p3TokensCount++;
                        Debug.LogError("player Number is : " + player.playerIndex + " and Tokens are : " + AIGameManager.Instance.p3TokensCount);
                        MatchManager.Instance.diceRollsRemaining++;
                    
                    if (AIGameManager.Instance.p3TokensCount == 4)
                    {
                        Debug.LogError("OOOhhhh!!!! you Lost");
                        AIGameManager.Instance.level_Failed_Panel.SetActive(true);
                        DiceAI.Instance.collider.gameObject.SetActive( false);
                        for (int i = 0; i < Constants.Instance.PlayerTurnHighlighters.Length; i++)
                        {

                            Constants.Instance.PlayerTurnHighlighters[i].SetActive(false);
                        }
                    }

                }
                if (player.playerIndex == 1)
                {
                    
                        AIGameManager.Instance.p1TokensCount++;
                        Debug.LogError("player Number is : " + player.playerIndex + " and Tokens are : " + AIGameManager.Instance.p1TokensCount);
                        MatchManager.Instance.diceRollsRemaining++;
                    
                    if (AIGameManager.Instance.p1TokensCount == 4)
                    {
                        Debug.LogError("Congratulations You Won !!!");
                        AIGameManager.Instance.level_Complete_Panel.SetActive(true);
                        DiceAI.Instance.collider.gameObject.SetActive(false);
                        for (int i = 0; i < Constants.Instance.PlayerTurnHighlighters.Length; i++)
                        {

                            Constants.Instance.PlayerTurnHighlighters[i].SetActive(false);
                        }
                    }
                }
                if (player.playerIndex == 2)
                {
                    
                        AIGameManager.Instance.p2TokensCount++;
                        Debug.LogError("player Number is : " + player.playerIndex + " and Tokens are : " + AIGameManager.Instance.p2TokensCount);
                        MatchManager.Instance.diceRollsRemaining++;
                    
                    if (AIGameManager.Instance.p2TokensCount ==4)
                    {
                        Debug.LogError("OOOhhhh!!!! you Lost");
                        AIGameManager.Instance.level_Failed_Panel.SetActive(true);
                        DiceAI.Instance.collider.gameObject.SetActive(false);
                        for (int i = 0; i < Constants.Instance.PlayerTurnHighlighters.Length; i++)
                        {

                        Constants.Instance.PlayerTurnHighlighters[i].SetActive(false);
                        }
                    }
                }
                if (player.playerIndex == 4)
                {
                   
                        AIGameManager.Instance.p4TokensCount++;
                        Debug.LogError("player Number is : " + player.playerIndex + " and Tokens are : " + AIGameManager.Instance.p4TokensCount);
                        MatchManager.Instance.diceRollsRemaining++;
                    
                    if (AIGameManager.Instance.p4TokensCount == 4)
                    {
                        Debug.LogError("OOOhhhh!!!! you Lost");
                        AIGameManager.Instance.level_Failed_Panel.SetActive(true);
                        DiceAI.Instance.collider.gameObject.SetActive(false);
                        for (int i = 0; i < Constants.Instance.PlayerTurnHighlighters.Length; i++)
                        {

                            Constants.Instance.PlayerTurnHighlighters[i].SetActive(false);
                        }
                    }

                }
               
                //UnityEditor.EditorApplication.isPaused = true;
                //GameManager.Instance.SetGameSpeed(GameManager.GameSpeed.Normal);
            }
            // else if we're not at a safe waypoint now, we can check for tokens to kill
            else if (!safeWaypoints.Contains(localWaypointIndex))
            {
                foreach (PlayerToken oneToken in PlayersManager.GetTokensAt(localWaypointIndex, player))
                {
                    // If this token isn't our player's token
                    if (oneToken.player != player)
                    {
                        Debug.LogError("Player " + player.playerIndex + " killed " + oneToken.player.playerIndex + "'s token, should get an extra turn too!");
                        //Debug.LogError("Player " + PlayersManager.Players[player.playerIndex - 1] + " killed " + oneToken.player.playerIndex + "'s token, should get an extra turn too!");
                        //UnityEditor.EditorApplication.isPaused = true;
                        //GameManager.Instance.SetGameSpeed(GameManager.GameSpeed.Normal);
                        // Creating killed token's die animation here to create systematic delays in game-actions
                        // TODO update this for networked game
                        
                        if(oneToken.player.playerIndex==1)
                        {
                            Instantiate(AIGameManager.Instance.deathffects[0], oneToken.transform);
                        }
                        else if(oneToken.player.playerIndex == 2)
                        {
                            Instantiate(AIGameManager.Instance.deathffects[1], oneToken.transform);
                        }
                        else if (oneToken.player.playerIndex == 3)
                        {
                            Instantiate(AIGameManager.Instance.deathffects[2], oneToken.transform);
                        }
                        else if (oneToken.player.playerIndex == 4)
                        {
                            Instantiate(AIGameManager.Instance.deathffects[3], oneToken.transform);
                        }
                        yield return new WaitForSeconds(Constants.delayBetweenTokenMoves);
                        oneToken.localWaypointIndex = -1;
                        SanUtils.PlaySound(Constants.Instance.sfxTokenKill);
                        yield return new WaitForSeconds(2);
                        oneToken.transform.position = oneToken.Base.position;

                        if (oneToken.player.playerIndex == 1)
                        {
                            Instantiate(AIGameManager.Instance.lifeEffects[0], oneToken.transform);
                        }
                        else if (oneToken.player.playerIndex == 2)
                        {
                            Instantiate(AIGameManager.Instance.lifeEffects[1], oneToken.transform);
                        }
                        else if (oneToken.player.playerIndex == 3)
                        {
                            Instantiate(AIGameManager.Instance.lifeEffects[2], oneToken.transform);
                        }
                        else if (oneToken.player.playerIndex == 4)
                        {
                            Instantiate(AIGameManager.Instance.lifeEffects[3], oneToken.transform);
                        }
                        yield return new WaitForSeconds(1);
                        yield return new WaitForSeconds(Constants.delayAfterTokenMoveComplete);

                        MatchManager.Instance.diceRollsRemaining++;
                    }
                }
            }

            MatchManager.Instance.NextTurn();
            yield return null;
        }
        public bool CanMove(int stepsToTake)
        {
            if ((Constants.LastWaypointIndex - localWaypointIndex) >= stepsToTake)
            {
                // if player is at base
                if (localWaypointIndex == -1)
                {
                    // if player is at base, they can move only if diceResult is 6
                    if (stepsToTake == 6)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
    }
}