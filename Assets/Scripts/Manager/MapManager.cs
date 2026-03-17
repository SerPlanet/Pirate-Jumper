using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    private const float Player_Distance_Spawn_Level = 100f;

    [Header("MoveMap")]
    [SerializeField] private float startSpeed;
    [SerializeField] private float maxSpeed;

    [SerializeField] private float acceleration;
    [SerializeField] private bool moveMap;

    [Header("New SegmentGeneration")]
    [SerializeField] private GameObject mapHolderObj;

    private Transform mapHolder;
    [SerializeField] private Transform player;

    [SerializeField] private List<MapPrefab> mapPrefabs = new List<MapPrefab>();
    [SerializeField] private List<MapPrefab> currentActiveMapPrefabs = new List<MapPrefab>();
    
    [Header ("ParalexEffect")]
    [SerializeField] private float parallaxMultiplierWaterBackground;
    [SerializeField] private float parallaxMultiplierWaterVoreground;
    [SerializeField] private float parallaxMultiplierSky;

    [SerializeField] private float parallexMultiplierPirateBay;
    [SerializeField] private GameObject skyBackgroundObj;
    [SerializeField] private GameObject waterBackgroundObj;
    [SerializeField] private GameObject waterVoregroundObj;

    [SerializeField] private GameObject piratBayBackgroundObj;

    [Header ("New SegmentPrivat Var")]
    private MapPrefab mapPrefab;
    private Transform lastMapPrefabPart;

    [Header ("ParralexSpawnedMaps")]
    private Transform skyBackground, waterBackground, waterVoreground,piratBayBackground;

    [Header ("Scores")]
    private ulong currentScore;
    private float scoreChecker;

     private Coroutine slowDownCoroutine;
    private bool itemInUse;
    private int prevNr;

    [Header ("Paralex Privat Var")]
    private float length, movementWater, movementSki, movementWaterVoreground, speed, movementPirateBayBackground;

    // Pixel Perfect Scroll
    private float scrollPosition;
    [SerializeField] private float pixelsPerUnit = 16f;
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
        speed = startSpeed;
    }

    private void Start() //Spawn the start Platform so from there it can go on
    {
        SpawnParalexBackground();
        mapHolder = Instantiate(mapHolderObj).transform;
        length = skyBackground.GetComponent<SpriteRenderer>().bounds.size.x;
        //SpawnMapPart(new Vector3(0,0,0)); 
        GameManager.OnGameStateChange += StartGame;
        GetPlayerFromManager();
        StopMap();
        SpawnManager.Instance.MapManagerRdy();
    }

    private void OnDestroy()
    {
         GameManager.OnGameStateChange -= StartGame;
    }
    private void Update()
    {   
       
        if (moveMap) //Var for dev reasons to stop scrolling
        {
            MoveMapSegments(); //Move every segment at the same time
            MoveParalexMap();
            if(Vector3.Distance(player.position, lastMapPrefabPart.position) < Player_Distance_Spawn_Level) //Check if the player is close to the end so you need to spawn more segments
            {
                SpawnMapPart(mapPrefab.GetEndPosition());
            }
            if (!itemInUse)
            {
                speed = Mathf.Lerp(speed, maxSpeed, acceleration * Time.deltaTime);
            }
             
        }
    }

    #region MapMovement
    #endregion
    //Helper Functions

    private void SpawnParalexBackground()
    {
       // skyBackground, waterBackground, waterVoreground,piratBayBackground;
       skyBackground = Instantiate(skyBackgroundObj).transform;
       waterBackground = Instantiate(waterBackgroundObj).transform;
       waterVoreground = Instantiate(waterVoregroundObj).transform;
       piratBayBackground = Instantiate(piratBayBackgroundObj).transform;

    }
    public void GivePlayerToMap(Transform playerTransform){player = playerTransform;}

    private void GetPlayerFromManager(){player = GameManager.Instance.GetPlayer().transform;}

    /* SpawnMapPart (Takes the endLocation of the last placed segment)
    First instantiate the new Platform and chose one randomly from the mapPrefabs. The location is the location given
    After that set the new last platform to this platform and add the new platform ind the currentActuveMapPrefabs so you can move it with the others

    */
    private void SpawnMapPart(Vector3 spawnPosition)
    {
        int randomSelection = Random.Range(0, mapPrefabs.Count);
        //int randomSelection = 9;//Random.Range(mapPrefabs.Count-1, mapPrefabs.Count-1);
        if(randomSelection == prevNr)
        {
            randomSelection = (randomSelection+1)%mapPrefabs.Count;
        }
        prevNr = randomSelection;
        lastMapPrefabPart = Instantiate(mapPrefabs[randomSelection].GetTransform(),spawnPosition, Quaternion.identity, mapHolder);
        mapPrefab = lastMapPrefabPart.GetComponent<MapPrefab>();
        currentActiveMapPrefabs.Add(mapPrefab);
        CheckIfOldPartCanDespawn(); //Check if old segments can leave
    }

    private void CheckIfOldPartCanDespawn()
    {
        if(currentActiveMapPrefabs.Count > 4)
        {
            currentActiveMapPrefabs[0].RemoveMapPrefab();
            currentActiveMapPrefabs.RemoveAt(0);
        }
    }


    private void MoveMapSegments()
    {


        foreach(MapPrefab mapPrefab in currentActiveMapPrefabs)
        {
             Transform currentTransform = mapPrefab.GetTransform();

            Vector3 pos = currentTransform.position;
            pos.x -= speed * Time.deltaTime;

            currentTransform.position = pos;
        }
        
        scoreChecker += speed*Time.deltaTime;
        if(scoreChecker >= 1)
        {
            scoreChecker = 0;
            currentScore ++;
            UIManager.Instance.SetCurrentSoreInGame(currentScore);
        }
    }

    private void MoveParalexMap()
    {
        movementSki += (speed *parallaxMultiplierSky)*Time.deltaTime;
        movementWater += (speed * parallaxMultiplierWaterBackground)*Time.deltaTime;
        movementWaterVoreground += (speed * parallaxMultiplierWaterVoreground)*Time.deltaTime;
        movementPirateBayBackground += (speed * parallexMultiplierPirateBay)*Time.deltaTime;

         if(movementPirateBayBackground >= 140)
        {
           // Debug.Log("--------RESEZT-----------" + waterBackground.position +","+ movementWater);
            piratBayBackground.position = new Vector3(52, piratBayBackground.position.y, piratBayBackground.position.z);
            movementPirateBayBackground = 0;
        }
        else
        {
              piratBayBackground.position += Vector3.left * speed * parallexMultiplierPirateBay * Time.deltaTime;
        }
        //Debug.Log("Var"+ movementWater + "," + movementSki + "Pos" + skyBackground.position + "," + waterBackground.position);
        if(movementSki >= length)
        {
           // Debug.Log("--------RESEZT-----------" + skyBackground.position +","+ movementSki);
           skyBackground.position = new Vector3(length, skyBackground.position.y, skyBackground.position.z);
           movementSki = 0;
        }
        else
        {
            skyBackground.position += Vector3.left * speed * parallaxMultiplierSky * Time.deltaTime;
        }
        if(movementWater >= length)
        {
            //Debug.Log("--------RESEZT-----------" + waterBackground.position +","+ movementWater);
            waterBackground.position = new Vector3(length, waterBackground.position.y, waterBackground.position.z);
            movementWater = 0;
        }
        else
        {
             waterBackground.position += Vector3.left * speed * parallaxMultiplierWaterBackground * Time.deltaTime;
        }

        if(movementWaterVoreground >= length)
        {
           // Debug.Log("--------RESEZT-----------" + waterBackground.position +","+ movementWater);
            waterVoreground.position = new Vector3(length, waterVoreground.position.y, waterVoreground.position.z);
            movementWaterVoreground = 0;
        }
        else
        {
             waterVoreground.position += Vector3.left * speed * parallaxMultiplierWaterVoreground * Time.deltaTime;
        }
        
        
       
    }

    private void DeleteAllSegments()
    {
        for(int i = currentActiveMapPrefabs.Count - 1; i >= 0; i--)
        {
            currentActiveMapPrefabs[i].RemoveMapPrefab();
            currentActiveMapPrefabs.RemoveAt(i);
        }
    }

    private void SetAllParametersToStandard()
    {
         // Score & Movement
        currentScore = 0;
        scoreChecker = 0;
        movementWater = 0;
        movementSki = 0;
        movementPirateBayBackground = 0;

        // Stoppe eventuell laufende Coroutine
        if (slowDownCoroutine != null)
        {
            StopCoroutine(slowDownCoroutine);
            slowDownCoroutine = null;
        }

        // Map Speed
        speed = startSpeed;
        itemInUse = false; // auch Item zurücksetzen

        // Parallax Hintergrund reset
        skyBackground.position = new Vector3(length, skyBackground.position.y, skyBackground.position.z);
        waterBackground.position = new Vector3(length, waterBackground.position.y, waterBackground.position.z);
        piratBayBackground.position = new Vector3(52, piratBayBackground.position.y, piratBayBackground.position.z);
    }

    private void LoadNewMapSegmentsAtStart()
    {
         SpawnMapPart(new Vector3(0,0,0)); 
    }

    private void StopMap()
    {
        moveMap = false;
    }

    private void StartMap()
    {
        moveMap = true;
    }

    private void ResetMap()
    {
        DeleteAllSegments();
        SetAllParametersToStandard();
        LoadNewMapSegmentsAtStart();
        GameManager.Instance.MapManagerIsReady();
    }

    private void TransmitScoreToGameManager()
    {
        GameManager.Instance.SetScore(currentScore);
    }


    public void StartGame(GameStates gameState)
    {
        switch (gameState){
            case(GameStates.StartGame):
            //StartMap();
            break;
            case(GameStates.GameEnds):
            StopMap();
            TransmitScoreToGameManager();
            break;
            case(GameStates.GameRunning):
            StartMap();
            break;
            case(GameStates.PauseGame):
            StopMap();
            break;
            case(GameStates.Loading):
            ResetMap();
            break;
            case (GameStates.Lobby):
                ResetMap();
            break;
            case (GameStates.Item):
                StopMap();
                
            break;
        }
    }

    #region items

    public void UseKanon()
    {
        itemInUse = true;
        speed = 25;
        StartMap();
    }
    public void StopUseItem()
    {
        float currSpeed = speed;
                currSpeed = currSpeed/2;
                if (currSpeed < startSpeed)
                {
                    speed = startSpeed;
                }
                else
                {
                    speed = currSpeed;
                }
        itemInUse = false;
    }

    public void UseHeiligenschein()
    {
        StopMap();
    }

    public void SlowDownBounce(float duration)
    {
    // Wenn schon eine Coroutine läuft → abbrechen
    if(slowDownCoroutine != null)
        StopCoroutine(slowDownCoroutine);

    // Neue Coroutine starten und speichern
    slowDownCoroutine = StartCoroutine(SlowDownRoutine(duration));
    }

    private IEnumerator SlowDownRoutine(float duration)
    {
        float originalSpeed = speed;
        float durationToSlow = 1f; // Zeit, um auf startSpeed zu kommen

        float elapsed = 0f;
        float fromSpeed = speed;
        float slowedSpeed = speed*0.8f;
        if (slowedSpeed < startSpeed)
        {
            slowedSpeed = startSpeed;
        }

        // Ease-Out runter auf startSpeed
        while(elapsed < durationToSlow)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / durationToSlow;

            // Ease-Out Kurve: schnelle Änderung am Anfang, langsamer am Ende
            t = 1f - Mathf.Pow(1f - t, 3); // Cubic Ease-Out
            speed = Mathf.Lerp(fromSpeed, slowedSpeed, t);

            yield return null;
            }

            speed = slowedSpeed; // sicherstellen, dass wir exakt am Ziel sind

            // Hold für holdDuration
            yield return new WaitForSeconds(duration);

            // Smooth wieder hoch auf originalSpeed (linear oder ebenfalls Ease-In)
            elapsed = 0f;
            fromSpeed = speed;
            float durationToRecover = 1f;

            while(elapsed < durationToRecover)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / durationToRecover;

                // Optional: Ease-In Kurve für sanftes Hochfahren
                t = Mathf.Pow(t, 2); // Quadratisch Ease-In
                speed = Mathf.Lerp(fromSpeed, originalSpeed, t);

                yield return null;
            }

            speed = originalSpeed; // sicherstellen
            slowDownCoroutine = null;
    }

   

    #endregion
}
