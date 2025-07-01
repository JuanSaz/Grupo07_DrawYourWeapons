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
    private IEnumerator MyStart()
    {
        yield return null;
        SpawnPlayers();
        onPlayerKilled += ManagePlayerKilled;
        SelectRandomMap();
        InstantiateWalls();
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

    private IEnumerator RestartRoundAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        UnloadWalls();
        SelectRandomMap();
        InstantiateWalls();
        OnRoundRestart.Invoke();
    }
    public void UnsubscribeAllObjects()
    {
        UpdateManager.Instance.UnsubscribeAll();
        activePlayers.Clear();
    }

    public void SelectRandomMap()
    {
        int random = UnityEngine.Random.Range(0, InstantiatorManager.Instance.objectsToPreload.Count);
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

