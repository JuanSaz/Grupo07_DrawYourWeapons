using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

[DefaultExecutionOrder(-1)]
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public List<Transform> spawnPoints = new List<Transform>();
    public List<MyBehaviorType> playerBehaviorTypes = new List<MyBehaviorType>();

    [SerializeField] private GameObject WinCanvas;

    private Vector2 screenBounds = new Vector2(10, 6);
    private List<PencilEntity> activePowerUps = new List<PencilEntity>(3);

    public Action<PlayerEntity> onPlayerKilled;
    private List<PlayerEntity> activePlayers = new List<PlayerEntity>();
    private List<PlayerEntity> totalPlayers = new List<PlayerEntity>();
    public List<PlayerEntity> ActivePlayers { get { return activePlayers; } set { activePlayers = value; } }
    public List<PlayerEntity> TotalPlayers { get { return totalPlayers; } set { totalPlayers = value; } }

    private List<WallEntity> activeWalls = new List<WallEntity>();

    [SerializeField] private int pointsToWin;

    private Coroutine RestartRoundCor;

    public UnityEvent OnRoundRestart = new UnityEvent();
    private LevelInfoSO currentLevelInfo;
    [SerializeField] private LevelInfoSO levelBordersInfo;
    private int previousRandomIndex = -1;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        StartCoroutine(MyStart());
    }

    public void InstantiatePowerUps()
    {
        ClearPowerUps();

        // Random 1-3 powerups
        int powerUpCount = UnityEngine.Random.Range(1, 4);

        for (int i = 0; i < powerUpCount; i++)
        {
            float x = UnityEngine.Random.Range(-screenBounds.x, screenBounds.x);
            float y = UnityEngine.Random.Range(-screenBounds.y, screenBounds.y);
            Vector3 spawnPosition = new Vector3(x, y, 0f);

            PencilEntity powerUp = (PencilEntity)InstantiatorManager.Instance.Create(MyBehaviorType.PencilPowerUp);

            if (powerUp == null) Debug.Log("asda");

            powerUp.EntityGameObject.transform.position = spawnPosition;
            powerUp.WakeUp();

            activePowerUps.Add(powerUp);
        }
    }

    private void ClearPowerUps()
    {
        for (int i = 0; i < activePowerUps.Count; i++)
        {
            if (activePowerUps[i] != null && activePowerUps[i].EntityGameObject != null)
            {
                activePowerUps[i].EntityGameObject.SetActive(false);
            }
        }
        activePowerUps.Clear();
    }
    private IEnumerator MyStart()
    {
        yield return null;
        SpawnPlayers();
        onPlayerKilled += ManagePlayerKilled;
        SelectRandomMap();
        InstantiateWalls();
        InstatiateLevelBorders();
        InstantiatePowerUps();
    }

    public void SpawnPlayers()
    {
        for (int i = 0; i < GameManager.Instance.GetPlayerAmount(); i++)
        {
            PlayerEntity newPlayer = (PlayerEntity)InstantiatorManager.Instance.Create(playerBehaviorTypes[i]);
            newPlayer.EntityGameObject.transform.SetPositionAndRotation(spawnPoints[i].position, spawnPoints[i].rotation);
            newPlayer.WakeUp();
            newPlayer.PlayerName = "Player " + (i + 1);
            totalPlayers.Add(newPlayer);
            activePlayers.Add(newPlayer);
            if (i > 4)
            {
                break;
            }
        }
    }

    public void SetWinner(PlayerEntity Winner)
    {
        UnsubscribeAllObjects();
        
        WinInfo info = WinCanvas.GetComponent<WinInfo>();

        info.SetWinnerNameAndScore(Winner.PlayerName, Winner.Score);
        WinCanvas.SetActive(true);
    }

    private void ManagePlayerKilled(PlayerEntity player)
    {
        activePlayers.Remove(player);

        if (activePlayers.Count == 1)
        {
            activePlayers[0].AddPoint();
            UIManager.Instance.onScoreChanged.Invoke(activePlayers[0]);
            ManageRoundRestart(activePlayers[0]);
        }
    }

    public void ManageRoundRestart(PlayerEntity survivour)
    {
        if (survivour.Score == pointsToWin)
        {
            SetWinner(survivour);
        }
        else
        {
            RestartRoundCor = StartCoroutine(RestartRoundAfterDelay(2f));
        }
    }

    private void InstatiateLevelBorders()
    {
        for (int i = 0; i < levelBordersInfo.wallPositions.Length; i++)
        {
            Entity borderEntity = InstantiatorManager.Instance.Create(MyBehaviorType.Border);
            borderEntity.EntityGameObject.transform.position = new Vector3(levelBordersInfo.wallPositions[i].x, levelBordersInfo.wallPositions[i].y, 0f);

            Vector3 currentEuler = borderEntity.EntityGameObject.transform.rotation.eulerAngles;
            borderEntity.EntityGameObject.transform.rotation = Quaternion.Euler(currentEuler.x, currentEuler.y, levelBordersInfo.wallPositions[i].z);
            borderEntity.WakeUp();
        }
    }

    private IEnumerator RestartRoundAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        UnloadWalls();
        SelectRandomMap();
        InstantiateWalls();
        InstantiatePowerUps();
        OnRoundRestart.Invoke();
    }
    public void UnsubscribeAllObjects()
    {
        UpdateManager.Instance.UnsubscribeAll();
        activePlayers.Clear();
    }
    public void SelectRandomMap()
    {
        int count = InstantiatorManager.Instance.objectsToPreload.Count;
        int random = UnityEngine.Random.Range(0, count);

        if (count > 1)
        {
            while (random == previousRandomIndex)
            {
                random = UnityEngine.Random.Range(0, count);
            }
        }

        previousRandomIndex = random;
        currentLevelInfo = (LevelInfoSO)InstantiatorManager.Instance.objectsToPreload[random].OperationHandle.Result;
    }
    private void InstantiateWalls()
    {
        for (int i = 0; i < currentLevelInfo.wallPositions.Length; i++)
        {
            WallEntity wallEntity = InstantiatorManager.Instance.wallPool.pool.Get();
            wallEntity.EntityGameObject.transform.position = currentLevelInfo.wallPositions[i];
            activeWalls.Add(wallEntity);

            Vector3 currentEuler = wallEntity.EntityGameObject.transform.rotation.eulerAngles;
            wallEntity.EntityGameObject.transform.rotation = Quaternion.Euler(currentEuler.x, currentEuler.y, currentLevelInfo.wallPositions[i].z);
            wallEntity.WakeUp();
        }
    }
    public void UnloadWalls()
    {
        for (int i = 0; i < activeWalls.Count; i++)
        {
            InstantiatorManager.Instance.wallPool.pool.Release(activeWalls[i]);
        }
    }
}

