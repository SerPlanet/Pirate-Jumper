using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public GameStates gameState;

    [SerializeField] private GameObject playerObject;

    public static event Action<GameStates> OnGameStateChange;

    public static event Action<bool> MagnetIsActive;
    public static GameManager Instance;

    private MovementScript player;
    private GameObject currPlayer;

    private ulong score;

    private ulong currentRunMoney;
    private ulong money;
    private ulong highscore;

    private bool mapIsReady;
    private bool playerIsReady;
    private bool gameManagerReady;

    private bool isDoubleXP, isMagnetAktiv;
     private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currPlayer = Instantiate(playerObject);
        player = currPlayer.GetComponent<MovementScript>();
        //MapManager.Instance.GivePlayerToMap(player.transform);
        highscore = SaveManager.Instance.LoadHighscore();
        money = SaveManager.Instance.LoadMoney();
        OpenApp();
    }
    #region Player

    public MovementScript GetPlayerScript(){return player;}

    public GameObject GetPlayer(){return currPlayer;}

    public void UseDoubleMoney(){isDoubleXP = true;}
    public void StopDoubleMoney(){isDoubleXP = false;}

    public void StartMagnet()
    {
        isMagnetAktiv = true;
        MagnetIsActive?.Invoke(isMagnetAktiv);
    }
    public void StopMagnet()
    {
        isMagnetAktiv = false;
        MagnetIsActive?.Invoke(isMagnetAktiv);
    }
    public bool  GetIsMagnetAktiv(){return isMagnetAktiv;}

    #endregion

    #region Lobby

    public bool OpenChestWithAmount(ulong amountPerChest)
    {
        if(amountPerChest < money)
        {
            money -= amountPerChest;
            return true;
        }
        else
        {
             return false;
        }
    }

    #endregion

    #region GameValues

    public void SetScore(ulong currentScore)
    {
        score = currentScore;
        if (CheckForHighScore())
        {
            //ShowInDeathUI
        }
        AddMoneyToBank();
        UIManager.Instance.SetUpDeathScreanUI(currentRunMoney, currentScore, highscore);
    }

    public void AddAmountMoneyIngame(int moneyToAdd)
    {
        if (!isDoubleXP)
        {
            currentRunMoney += (ulong) moneyToAdd;
            UIManager.Instance.SetCurrentMoneyInGameScore(currentRunMoney);
        }
        else
        {
            currentRunMoney += (ulong) moneyToAdd*2;
            UIManager.Instance.SetCurrentMoneyInGameScore(currentRunMoney);
        }
        
    }

    private void SetHighScore(ulong newHighscore)
    {
        highscore = newHighscore;
        SaveManager.Instance.SaveHighscore(highscore);
    }

    private bool CheckForHighScore()
    {
        if(highscore < score)
        {
            SetHighScore(score);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AddMoneyToBank()
    {
        money += currentRunMoney;
        SaveManager.Instance.SaveMoney(money);
    }

    private void ResetDataForRun()
    {
        currentRunMoney = 0;
        score = 0;
    }

    public ulong GetCurrentScore(){ return score;}
    public ulong GetCurrentMoneyFromRun(){return currentRunMoney;}

    public ulong GetMoney() {return money;}
    public ulong GetHighscore(){return highscore;}

    #endregion


    #region GameState

    private void ChangeGameState(GameStates newGameState)
    {
        gameState = newGameState;
        GameStateChanged();
    }

    private void GameStateChanged()
    {
        OnGameStateChange?.Invoke(gameState);
    }

    private void OpenApp()
    {
        ChangeGameState(GameStates.OpenApp);
        //CameraManager.Instance.SetPlayerTransfrom(currPlayer.transform);
        SpawnManager.Instance.GameManagerRdy();
    }

    public void StartGame()
    {
        playerIsReady = false;
        mapIsReady = false;
        gameManagerReady = false;
        ChangeGameState(GameStates.StartGame);
    }

    public void GameRunning()
    {
        ChangeGameState(GameStates.GameRunning);
    }
    public void PauseGame()
    {
        ChangeGameState(GameStates.PauseGame);
    }

    public void GameEnds()
    {
        isMagnetAktiv = false;
        ChangeGameState(GameStates.GameEnds);
       
    }

    public void GameLobby()
    {
        ResetDataForRun();
        ChangeGameState(GameStates.Lobby);
    }

    public void UseItem()
    {
        ChangeGameState(GameStates.Item);
    }

    public void LoadGame()
    {
        StartCoroutine(WaitForSegmentsToBeReady());
        ChangeGameState(GameStates.Loading);
        ResetDataForRun();
        gameManagerReady = true;
        
    }

    public void OpenChestScrean()
    {
        ChangeGameState(GameStates.OpenChestScrean);
    }

    public void OpenCharachterSelectScrean()
    {
        ChangeGameState(GameStates.CharachterSelectScrean);
    }

    public void MapManagerIsReady()
    {
        mapIsReady = true;
    }
    public void PlayerIsReady()
    {
        playerIsReady = true;
    }

    private IEnumerator WaitForSegmentsToBeReady()
    {
        yield return new WaitUntil(() => playerIsReady && mapIsReady&&gameManagerReady);

        //Debug.Log("Beide Systeme sind bereit!");
        
        // Hier geht es weiter
        StartGame();
    }
    #endregion
}


public enum GameStates
{
    OpenApp,
    Loading,
    StartGame,
    GameRunning,
    PauseGame,
    GameEnds,
    Lobby,
    Shop,
    Item,
    CharachterSelectScrean,
    OpenChestScrean
}
