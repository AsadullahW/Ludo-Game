﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
public class GameManager : MonoBehaviour {

    #region singleton
    public static GameManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one GameManager in the scene");
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [SerializeField]
    public static int _numberOfPlayers = 4;
    public static int NumberOfPlayers
    {
        get { return _numberOfPlayers; }
        set { _numberOfPlayers = Mathf.Clamp(value, 2, 4); }
    }


    [SerializeField]
    private int _currentPlayerIndex;
    private int CurrentPlayerIndex
    {
        get { return _currentPlayerIndex; }
        set
        {
            if (value >= NumberOfPlayers)
            {
                _currentPlayerIndex = 0;
            }
            else
            {
                _currentPlayerIndex = value;
            }
        }
    }

    public float minRangeError;
    public float smoothness;

    public static int totalPlayerInMatch = 0;


    private bool hasKilled = false;

    public List<Player> players = new List<Player>();
    public Player currentPlayer;

    public bool waitingForRoll = true;
    public static bool is_2vs2 = false;
    public DiceCube dice;

    public WinningUI winningUI;
    public GameObject gameOverUI;

    [Header("Token Prefab")]
    public GameObject blueTokenPrefab = null;
    public GameObject greenTokenPrefab = null;
    public GameObject redTokenPrefab = null;
    public GameObject yellowTokenPrefab = null;

    [Header("Token Spawn")]
    public Transform[] blueSpawnNodesTransforms = null;
    public Transform[] greenSpawnNodesTransforms = null;
    public Transform[] redSpawnNodesTransforms = null;
    public Transform[] yellowSpawnNodesTransforms = null;

    [Header("Death effects prefabs")]
    public GameObject blueDeathEffectPrefab = null;
    public GameObject greenDeathEffectPrefab = null;
    public GameObject redDeathEffectPrefab = null;
    public GameObject yellowDeathEffectPrefab = null;

    [Header("Life effects prefabs")]
    public GameObject blueLifeEffectPrefab = null;
    public GameObject greenLifeEffectPrefab = null;
    public GameObject redLifeEffectPrefab = null;
    public GameObject yellowLifeEffectPrefab = null;

    [Header("Token Holder")]
    public Transform blueToken_Holder = null;
    public Transform redToken_Holder = null;
    public Transform greenToken_Holder = null;
    public Transform yellowToken_Holder = null;

    [SerializeField] private GameObject winPanel = null;
    [SerializeField] private Text winTxt = null;

    private PhotonView View;

    private void Start()
    {
        totalPlayerInMatch = NumberOfPlayers;
        SetupGame();
        MainMenuUI.Instance.Select_team(1);
        View = GetComponent<PhotonView>();
    }
    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 4.4f);
    }
    public void BeginTurn()
    {
        int interactablesNumber = 0;
        Node nextNode;
        foreach (Token token in currentPlayer.tokens)
        {
            if (token.tokenStatus == TokenStatus.LOCKED_IN_SPAWN && dice.value == 6)
            {
                token.originalSpawnNodeComponent.interactable = true;
                token.tokenTransform.GetChild(0).gameObject.SetActive(true);
                interactablesNumber++;
                continue;
            }

            if(token.tokenStatus == TokenStatus.FREE_TO_MOVE)
            {
                nextNode = token.GetParentNodeComponent();
                for (int i = 1; i <= dice.value; i++)
                {
                    nextNode = nextNode.GetNextNode(currentPlayer.playerType);
                    if(nextNode == null)
                    {
                        break;
                    }
                }
                if(nextNode != null)
                {
                    token.GetParentNodeComponent().interactable = true;
                    token.tokenTransform.GetChild(0).gameObject.SetActive(true);
                    interactablesNumber++;
                }
            }
        }
        if (interactablesNumber == 0)
        {
            currentPlayer = GetNextPlayer();
            waitingForRoll = true;
        }
        
        if(interactablesNumber == 1)
        {
            foreach (Token token in currentPlayer.tokens)
            {
                if(token.tokenStatus == TokenStatus.LOCKED_IN_SPAWN && dice.value == 6)
                {
                  //  View.RPC(nameof(PlayWithChosenToken_func), RpcTarget.AllBuffered, token);//asad

                  //  PlayWithChosenToken_func(token);//asad
                    StartCoroutine(PlayWithChosenToken(token));
                    break;
                }
                if(token.tokenStatus == TokenStatus.FREE_TO_MOVE)
                    if(token.GetParentNodeComponent().interactable == true)
                    {
                      //  View.RPC(nameof(PlayWithChosenToken_func), RpcTarget.AllBuffered,token);//asad

                      //  PlayWithChosenToken_func(token);//asad
                        StartCoroutine(PlayWithChosenToken(token));
                        break;
                    }
            }
        }

    }

    public void WinningMessageShow()
    {
        if(players.Count.Equals(1))
        {
            winPanel.SetActive(true);
            winTxt.text = "Win Player is " + players[0].name;
            Debug.Log("Win Player is" + players[0].name);
        }

    }
    [PunRPC]
    public void PlayWithChosenToken_func(Token token)
    {
        StartCoroutine(PlayWithChosenToken(token));
    }
    public IEnumerator PlayWithChosenToken(Token token)
    {

        Debug.Log("Move Token");

        ResetInteractables();
        // The chosen token can only be ready to be spawned (in which case the player rolled 6) 
        // or a free token that CAN move taking rolled number into account.

        if (token.tokenStatus == TokenStatus.LOCKED_IN_SPAWN)
        // The chosen token is not yet spawned. This spawns it. Same player will play the next turn.
        {
            token.originalSpawnNodeComponent.interactable = false;
            token.Spawn();
            token.GetParentNodeComponent().AddPlayer(token);
            waitingForRoll = true;
        }
        else
        // The chosen token is free to move in this case.
        {
            // Finding path minus the last step.
            Vector3 direction;
            Vector3 targetPosition;
            Node nextNode = token.GetParentNodeComponent();
            token.GetParentNodeComponent().RemovePlayer(token);
            for (int i = 0; i < dice.value - 1; i++)
            {
                nextNode = nextNode.GetNextNode(currentPlayer.playerType);
                //move towards it
                if (nextNode.IsEmpty())
                    targetPosition = nextNode.GetPosition();
                else
                    targetPosition = nextNode.GetUpPosition();
                while (true)
                {
                    token.tokenTransform.localScale = Vector3.Slerp(token.tokenTransform.localScale, token.originalScale, smoothness * Time.deltaTime);
                    direction = targetPosition - token.tokenTransform.position;
                    if (direction.magnitude <= minRangeError)
                        break;
                    direction = Vector3.Slerp(Vector3.zero, direction, smoothness * Time.deltaTime);
                    token.tokenTransform.Translate(direction);
                    yield return 0;
                }
            }

            // Last step is an edge case. 4 scenarios may take place
            // 1) Next node allows killing, is not empty and nothing to kill (friendly tokens)
            // 2) Next node doesn't allow killing and is not empty
            // 3) Next node allows killing, is not empty and there are token(s) to kill
            // 4) Next node is empty
            // 1 & 2 can be treated together 3 & 4 also
            // Last step code
            nextNode = nextNode.GetNextNode(currentPlayer.playerType);
            if ((nextNode.allowsKilling && !nextNode.IsEmpty() && nextNode.GetPlayerToKill(token) == null) ||
                !nextNode.allowsKilling && !nextNode.IsEmpty())
            {
                nextNode.AddPlayer(token);
            }
            else
            {
                // Move normally.
                // Kill any opponent token if possible.
                while (true)
                {
                    if (token.IsColliding())
                    {
                        // Kill opponent token(s)
                        Token opponentToken = nextNode.GetPlayerToKill(token);
                        while(opponentToken != null)
                        {
                            nextNode.RemovePlayer(opponentToken);
                            StartCoroutine(KillOpponent(opponentToken));
                            opponentToken = nextNode.GetPlayerToKill(token);
                            hasKilled = true;
                        }
                    }
                    direction = nextNode.GetPosition() - token.tokenTransform.position;
                    if (direction.magnitude <= minRangeError)
                        break;
                    direction = Vector3.Slerp(Vector3.zero, direction, smoothness * Time.deltaTime);
                    token.tokenTransform.Translate(direction);
                    yield return 0;
                }
                
                nextNode.AddPlayer(token);
            }

            // Did we win? Is the game over?
            if (token.GetParentNodeComponent().GetNextNode(currentPlayer.playerType) == null)
            {
                token.tokenStatus = TokenStatus.WON;
                if (currentPlayer.HasWon())
                {
                    if (GameIsOver())
                    {
                        EndGame();
                        yield break;
                    }
                    else
                    {
                        winningUI.AnimateWinnnerText(currentPlayer.playerType);
                    }
                }
            }

            // Prepare for the next turn.
            if (!(token.tokenStatus == TokenStatus.WON && !currentPlayer.HasWon() || dice.value == 6 || hasKilled))
            {
                currentPlayer = GetNextPlayer();
            }
            hasKilled = false;
            waitingForRoll = true;
        }
    }
    
    IEnumerator KillOpponent(Token opponentToken)
    {
        // Instanciate death effect
        GameObject deathEffect = null;
        switch (opponentToken.tokenType)
        {
            case PlayerType.BLUE:
                deathEffect = Instantiate(blueDeathEffectPrefab, opponentToken.tokenTransform.position, opponentToken.tokenTransform.rotation);
                break;
            case PlayerType.GREEN:
                deathEffect = Instantiate(greenDeathEffectPrefab, opponentToken.tokenTransform.position, opponentToken.tokenTransform.rotation);
                break;
            case PlayerType.RED:
                deathEffect = Instantiate(redDeathEffectPrefab, opponentToken.tokenTransform.position, opponentToken.tokenTransform.rotation);
                break;
            case PlayerType.YELLOW:
                deathEffect = Instantiate(yellowDeathEffectPrefab, opponentToken.tokenTransform.position, opponentToken.tokenTransform.rotation);
                break;
        }
        Destroy(deathEffect, 3f);
        opponentToken.tokenTransform.GetComponent<MeshRenderer>().enabled = false;
        opponentToken.Despawn();
        yield return new WaitForSeconds(1f);
        opponentToken.tokenTransform.localScale = opponentToken.originalScale;
       // here_me opponentToken.tokenTransform.GetComponent<MeshRenderer>().enabled = true;
        // Instanciate life effect
        GameObject lifeEffect = null;
        switch (opponentToken.tokenType)
        {
            case PlayerType.BLUE:
                lifeEffect = Instantiate(blueLifeEffectPrefab, opponentToken.tokenTransform.position, opponentToken.tokenTransform.rotation);
                break;
            case PlayerType.GREEN:
                lifeEffect = Instantiate(greenLifeEffectPrefab, opponentToken.tokenTransform.position, opponentToken.tokenTransform.rotation);
                break;
            case PlayerType.RED:
                lifeEffect = Instantiate(redLifeEffectPrefab, opponentToken.tokenTransform.position, opponentToken.tokenTransform.rotation);
                break;
            case PlayerType.YELLOW:
                lifeEffect = Instantiate(yellowLifeEffectPrefab, opponentToken.tokenTransform.position, opponentToken.tokenTransform.rotation);
                break;
        }
        Destroy(lifeEffect, 3f);
    }

    void ResetInteractables()
    {
        foreach (Token token in currentPlayer.tokens)
        {
            if(token.tokenStatus == TokenStatus.LOCKED_IN_SPAWN)
            {
                token.originalSpawnNodeComponent.interactable = false;
                token.tokenTransform.GetChild(0).gameObject.SetActive(false);
            }
            if(token.tokenStatus == TokenStatus.FREE_TO_MOVE)
            {
                token.GetParentNodeComponent().interactable = false;
                token.tokenTransform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    Player GetNextPlayer()
    {
        CurrentPlayerIndex++;
        if (CurrentPlayerIndex >= players.Count)
            CurrentPlayerIndex = 0;
        while (players[CurrentPlayerIndex].HasWon())
            CurrentPlayerIndex++;
        return players[CurrentPlayerIndex];
    }
    public Player RefreshNextPlayerState()
    {
        if (CurrentPlayerIndex >= players.Count)
            CurrentPlayerIndex = 0;
        return players[CurrentPlayerIndex];
    }
    void SetupGame()
    {
        switch (NumberOfPlayers)
        {
            case 2:

                for(int i=0;i<blueSpawnNodesTransforms.Length;i++)
                {
                    blueSpawnNodesTransforms[i].gameObject.SetActive(is_2vs2);
                    greenSpawnNodesTransforms[i].gameObject.SetActive(is_2vs2);
                }

                players.Add(SetupPlayer(PlayerType.BLUE, blueTokenPrefab, blueSpawnNodesTransforms));
                players.Add(SetupPlayer(PlayerType.GREEN, greenTokenPrefab, greenSpawnNodesTransforms));

                break;

            case 3:

                for (int i = 0; i < blueSpawnNodesTransforms.Length; i++)
                {
                    blueSpawnNodesTransforms[i].gameObject.SetActive(!is_2vs2);
                    redSpawnNodesTransforms[i].gameObject.SetActive(!is_2vs2);
                    greenSpawnNodesTransforms[i].gameObject.SetActive(!is_2vs2);
                }

                players.Add(SetupPlayer(PlayerType.BLUE, blueTokenPrefab, blueSpawnNodesTransforms));
                players.Add(SetupPlayer(PlayerType.RED, redTokenPrefab, redSpawnNodesTransforms));
                players.Add(SetupPlayer(PlayerType.GREEN, greenTokenPrefab, greenSpawnNodesTransforms));

                break;

            case 4:

                for (int i = 0; i < blueSpawnNodesTransforms.Length; i++)
                {
                    blueSpawnNodesTransforms[i].gameObject.SetActive(!is_2vs2);
                    redSpawnNodesTransforms[i].gameObject.SetActive(!is_2vs2);
                    greenSpawnNodesTransforms[i].gameObject.SetActive(!is_2vs2);
                    yellowSpawnNodesTransforms[i].gameObject.SetActive(!is_2vs2);
                }

                players.Add(SetupPlayer(PlayerType.BLUE, blueTokenPrefab, blueSpawnNodesTransforms));
                players.Add(SetupPlayer(PlayerType.RED, redTokenPrefab, redSpawnNodesTransforms));
                players.Add(SetupPlayer(PlayerType.GREEN, greenTokenPrefab, greenSpawnNodesTransforms));
                players.Add(SetupPlayer(PlayerType.YELLOW, yellowTokenPrefab, yellowSpawnNodesTransforms));

                break;
        }
        //   CurrentPlayerIndex = Random.Range(0, NumberOfPlayers);
        CurrentPlayerIndex = 0;
        currentPlayer = players[CurrentPlayerIndex];
    }

    Player SetupPlayer(PlayerType _playerType, GameObject tokenPrefab, Transform[] _spawnNodes)
    {
        Transform[] tokenTransforms = new Transform[4];

        switch(_playerType)
        {
            case PlayerType.BLUE:

                for (int i = 0; i < 4; i++)
                {
                    GameObject token = Instantiate(tokenPrefab,blueToken_Holder);
                    tokenTransforms[i] = token.transform;
                }
                break;

            case PlayerType.RED:

                for (int i = 0; i < 4; i++)
                {
                    GameObject token = Instantiate(tokenPrefab, redToken_Holder);
                    tokenTransforms[i] = token.transform;
                }
                break; 
            
            case PlayerType.GREEN:

                for (int i = 0; i < 4; i++)
                {
                    GameObject token = Instantiate(tokenPrefab, greenToken_Holder);
                    tokenTransforms[i] = token.transform;
                }
                break;

            case PlayerType.YELLOW:

                for (int i = 0; i < 4; i++)
                {
                    GameObject token = Instantiate(tokenPrefab, yellowToken_Holder);
                    tokenTransforms[i] = token.transform;
                }
                break;

        }
       
        return new Player(_playerType, _spawnNodes, tokenTransforms);
    }

    void EndGame()
    {
        Debug.Log("GAME OVER!");
        gameOverUI.SetActive(true);
    }

    public bool GameIsOver()
    {
        int winners = 0;
        foreach (Player player in players)
        {
            if (player.HasWon())
                winners++;
        }
        return winners == NumberOfPlayers - 1;
    }

}