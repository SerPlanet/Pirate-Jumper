using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    [SerializeField] private List<GameObject> allManagers;
    [SerializeField] private GameObject gameManagerObj;
    [SerializeField] private GameObject uiManagerObj;
    [SerializeField] private Transform managerHolder;

    [SerializeField] private GameObject CanvasOpenApp;

    public bool test;
    private bool gameManager, audioManager, charachterManager, MapManager, uIManager;
    int rdyCount = 0;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(WaitTillUIAndGameManager());
        SpawnGameManager();
        StartCoroutine(WaitTillEverythingIsSpawned());
    }

    private void SpawnGameManager()
    {
        Instantiate(gameManagerObj,managerHolder);
        Instantiate(uiManagerObj, managerHolder);
    }

    private void SpawnEverthingElse()
    {
        foreach(GameObject obj in allManagers)
        {
            Instantiate(obj, managerHolder);
        }
    }
    private void LoadVisuals()
    {
        if (uIManager)
        {
            UIManager.Instance.SetLoadingValue(rdyCount);
        }
    }
    public void GameManagerRdy()
    {
        gameManager = true;
        rdyCount++;
        LoadVisuals();
    }
    public void UIManagerRdy(){
        uIManager = true;rdyCount++;
        LoadVisuals();
    }

    public void AudiManagerRdy(){
        audioManager = true;rdyCount++;
        LoadVisuals();
    }

    public void CharachterManagerRdy(){
        charachterManager = true;rdyCount++;
        LoadVisuals();
    }

    public void MapManagerRdy(){
        MapManager = true;rdyCount++;
        LoadVisuals();
    }

    private IEnumerator WaitTillUIAndGameManager()
    {
        while(!(gameManager && uIManager && test))
        {
            yield return null;
        }
        SpawnEverthingElse();
    }
    private IEnumerator WaitTillEverythingIsSpawned()
    {
        while(!(gameManager && audioManager && charachterManager && MapManager && uIManager))
        {
            yield return null;
        }
        //GameManager.Instance.GameLobby();
        UIManager.Instance.GameCanContinue();
        DestroyEverthingUsless();
    }

    private void DestroyEverthingUsless()
    {
        Destroy(CanvasOpenApp);
        Destroy(gameObject);
    }
}
