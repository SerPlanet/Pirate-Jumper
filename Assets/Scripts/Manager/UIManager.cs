using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject canvasUIObj;
    [SerializeField] private GameObject pauseUIObj;
    [SerializeField] private GameObject deathUIObj;
    [SerializeField] private GameObject gameUIObj;
    [SerializeField] private GameObject chestOpenUIObj;
    [SerializeField] private GameObject charachterSelectUIObj;
    [SerializeField] private GameObject lobbyUIObj;
    [SerializeField] private GameObject creditsUIObj;

    [SerializeField] private GameObject optionsUIObj;
    [SerializeField] private GameObject LoadingStartObj;


    [Header("UI")]
    [SerializeField] private PauseMenuUI pauseUI;
    [SerializeField] private DeathScreanUI deathUI;
    [SerializeField] private InGameUI gameUI;
    [SerializeField] private ChestOpenUI chestOpenUI;
    [SerializeField] private CharachterSelectUI charachterSelectUI;
    [SerializeField] private LobbyScreanUI lobbyUI;
    [SerializeField] private CreditUI creditUI;
    [SerializeField] private OptionsUI optionsUI;
    [SerializeField] private LoadingStartUI loadingStartUI;

    [Header("DeathUI")]
    [SerializeField] private float timeTillEnd = 3f;

    private Transform canvasUI;

    private Coroutine reviveCoroutine;

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
    void Start()
    {
        SpawnAllUI();
        GameManager.OnGameStateChange += GameStateChanged;
        HideAll();
        SpawnManager.Instance.UIManagerRdy();
    }
    private void OnDestroy()
    {
         GameManager.OnGameStateChange -= GameStateChanged;
    }

    private void SpawnAllUI()
    {
        canvasUI = Instantiate(canvasUIObj).transform;
        loadingStartUI = Instantiate(LoadingStartObj, canvasUI).GetComponent<LoadingStartUI>();
        lobbyUI = Instantiate(lobbyUIObj,canvasUI).GetComponent<LobbyScreanUI>();
        chestOpenUI = Instantiate(chestOpenUIObj,canvasUI).GetComponent<ChestOpenUI>();
        charachterSelectUI = Instantiate(charachterSelectUIObj,canvasUI).GetComponent<CharachterSelectUI>();
        gameUI = Instantiate(gameUIObj,canvasUI).GetComponent<InGameUI>();
        pauseUI = Instantiate(pauseUIObj,canvasUI).GetComponent<PauseMenuUI>();
        deathUI = Instantiate(deathUIObj,canvasUI).GetComponent<DeathScreanUI>();
        creditUI = Instantiate(creditsUIObj,canvasUI).GetComponent<CreditUI>();
        optionsUI = Instantiate(optionsUIObj, canvasUI).GetComponent<OptionsUI>();
       
    }
    #region LoadingUI
    public void SetLoadingValue(int rdyCount)
    {
        loadingStartUI.SetLoadingSlider(rdyCount);
    }

    public void GameCanContinue()
    {
        loadingStartUI.CanContinue();
    }
    #endregion

    #region LobbyUI

     public void SetUpLobbyScreanUI(ulong currentMoney, ulong highscore)
    {
        SetLobbyScreanMoney(currentMoney);
        SetLobbyScreanScoreighScore(highscore);
    }
    public void SetLobbyScreanMoney(ulong currentMoney){lobbyUI.SetMoney(currentMoney);}
    public void SetLobbyScreanScoreighScore(ulong highscore){lobbyUI.SetHighScore(highscore);}

    public void SetUpCharachterUI(){charachterSelectUI.CreateAllCharachters();}

    public void SetNewUnlockedCharachter(int i){charachterSelectUI.UpdateCharachterUnlock(i);}

    #endregion

    #region InGameUI

    public void SetCurrentMoneyInGameScore(ulong money){gameUI.SetMoney(money);}
    public void SetCurrentSoreInGame(ulong score){gameUI.SetScore(score);}

    public void SetUpItem(Sprite iconItem, float itemDuration){gameUI.SetUpItem(iconItem, itemDuration);}

    public void SetUpItemTime(float i){gameUI.SetTimeItem(i);}

    public void HideItem(){gameUI.HideItem();}

    public void SetUpPauseUI(bool musicUI, bool sfxUI)
    {
        pauseUI.SetSFXButton(sfxUI);
        pauseUI.SetMusicButton(musicUI);
    }

    #endregion

    #region DeathUI

    public void StartReviveUIAfterDeath()
    {
        gameUI.ShowAfterDeathReviveOption();
        if(reviveCoroutine == null)
        {
             reviveCoroutine = StartCoroutine(WaitTimeTillReviveEnds());
        }
      
    }

    public void PlayerRevived()
    {
        StopCoroutine(reviveCoroutine);
        reviveCoroutine = null;
    }
    private IEnumerator WaitTimeTillReviveEnds()
    {
        float currTime = 0;

        while(currTime < timeTillEnd)
        {
            currTime += Time.deltaTime;
            gameUI.SetTimerOfClock(currTime/timeTillEnd);
            yield return null;
        }
        gameUI.HideAfterDeathReviveOption();
        GameManager.Instance.GameEnds();
       //StopCoroutine(reviveCoroutine);
       reviveCoroutine = null;
    }
    public void SetUpDeathScreanUI(ulong currentMoney, ulong currentScore, ulong highscore)
    {
        SetDeathScreanMoney(currentMoney);
        SetDeathScreanScore(currentScore);
        SetDeathScreanScoreighScore(highscore);
    }
    public void SetDeathScreanMoney(ulong currentMoney){deathUI.SetMoney(currentMoney);}
    public void SetDeathScreanScore(ulong currentScore){deathUI.SetScore(currentScore);}
    public void SetDeathScreanScoreighScore(ulong highscore){deathUI.SetHighScore(highscore);}
    #endregion


    #region  Hide/ShowUI
    private void ShowPauseUI()
    {
        pauseUI.ShowUI();
    }

    private void HidePauseUI()
    {
        pauseUI.HideUI();
    }

    private void ShowDeathUI()
    {
        deathUI.ShowUI();
    }

    private void HideDeathUI()
    {
        deathUI.HideUI();
    }

    private void ShowGameUI()
    {
        gameUI.ShowUI();
    }

    private void HideGameUI()
    {
        gameUI.HideUI();
    }

    private void ShowChestOpenUI()
    {
        chestOpenUI.ShowUI();
    }

    private void HideChestOpenUI()
    {
        chestOpenUI.HideUI();
    }

    private void ShowCharachterSelectUI()
    {
        charachterSelectUI.ShowUI();
    }

    private void HideCharachterSelectUI()
    {
        charachterSelectUI.HideUI();
    }

    private void ShowLobbyUI()
    {
        SetUpLobbyScreanUI(GameManager.Instance.GetMoney(), GameManager.Instance.GetHighscore());
        //charachterSelectUI.Reset();
        lobbyUI.ShowUI();
    }

    public void ShowCreditUI()
    {
        creditUI.ShowUI();
    }

    private void HideCreditUI()
    {
        creditUI.HideUI();
    }

    public void ShowOptiontUI()
    {
        optionsUI.ShowUI();
    }

    private void HideOptionUI()
    {
        optionsUI.HideUI();
    }

    private void HideLobbyUI(){lobbyUI.HideUI();}

    private void HideAll()
    {
        HideGameUI();
        HideDeathUI();
        HidePauseUI();
        HideLobbyUI();
        HideCharachterSelectUI();
        HideChestOpenUI();
        HideCreditUI();
        HideOptionUI();
    }

    private void ResetAll()
    {
        deathUI.Reset();
        gameUI.Reset();
        lobbyUI.Reset();
        charachterSelectUI.Reset();
        chestOpenUI.Reset();
    }

    #endregion

    private void GameStateChanged(GameStates gameState)
    {
        switch (gameState){
            case(GameStates.StartGame):
                gameUI.Reset();
                ShowGameUI();
            
            break;
            case(GameStates.GameEnds):
                ShowDeathUI();
                HidePauseUI();
                HideGameUI();
            break;
            case(GameStates.GameRunning):
                HideLobbyUI();
                HidePauseUI();
                
            break;
            case(GameStates.PauseGame):
                ShowPauseUI();
            break;
            case(GameStates.Loading):
                ResetAll();
                HideAll();
            
            break;
            case(GameStates.Lobby):
                HideAll();
                ShowLobbyUI();
                charachterSelectUI.Reset();
                //ShowCharachterSelectUI();
            break;
            case GameStates.Item:
            break;
            case GameStates.CharachterSelectScrean:
                ShowCharachterSelectUI();
            break;
            case GameStates.OpenChestScrean:
                ShowChestOpenUI();
            break;
        }
    }


}
