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
    private bool gameManager, audioManager, charachterManager, MapManager, UIManager;
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

    public void GameManagerRdy(){gameManager = true;}
    public void UIManagerRdy(){UIManager = true;}

    public void AudiManagerRdy(){audioManager = true;}

    public void CharachterManagerRdy(){charachterManager = true;}

    public void MapManagerRdy(){MapManager = true;}

    private IEnumerator WaitTillUIAndGameManager()
    {
        while(!(gameManager && UIManager && test))
        {
            yield return null;
        }
        SpawnEverthingElse();
    }
    private IEnumerator WaitTillEverythingIsSpawned()
    {
        while(!(gameManager && audioManager && charachterManager && MapManager && UIManager))
        {
            yield return null;
        }
        GameManager.Instance.GameLobby();
        DestroyEverthingUsless();
    }

    private void DestroyEverthingUsless()
    {
        Destroy(CanvasOpenApp);
        Destroy(gameObject);
    }
}
