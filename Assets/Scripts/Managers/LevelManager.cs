using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;



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

    private AsyncOperationHandle<GameObject> currentLevelInstanceHandle;
    [SerializeField] private AssetReference[] levelObstacles;
    private AssetReference levelToInstantiate;
    private GameObject currentLevelReference;

    public List<PlayerEntity> ActivePlayers { get { return activePlayers; } set { activePlayers = value; } }
    public List<PlayerEntity> TotalPlayers { get { return totalPlayers; } set { totalPlayers = value; } }

    [SerializeField] private int pointsToWin;

    private Coroutine RestartRoundCor;

    public UnityEvent OnRoundRestart = new UnityEvent();

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
        SpawnLevel();
        SpawnPlayers();
        onPlayerKilled += ManagePlayerKilled;
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

    public void SpawnLevel()
    {
        PreloadLevel();
    }

    private void PreloadLevel()
    {
        if (currentLevelInstanceHandle.IsValid())
        {
            UnloadLevel();
        }

        int randomIndex = UnityEngine.Random.Range(0, levelObstacles.Length);
        levelToInstantiate = levelObstacles[randomIndex];

        currentLevelInstanceHandle = levelToInstantiate.InstantiateAsync();
        currentLevelInstanceHandle.Completed += OnLevelInstantiatedCompleted;
    }

    private void OnLevelInstantiatedCompleted(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Nivel instanciado correctamente.");
        }
        else
        {
            Debug.LogError("La instancia del nivel ha fallado.");
        }
    }


    private void UnloadLevel()
    {
        if (currentLevelInstanceHandle.IsValid())
        {
            Addressables.ReleaseInstance(currentLevelInstanceHandle);
            currentLevelInstanceHandle = default;
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
        SpawnLevel();
        OnRoundRestart.Invoke();
    }

    public void UnsubscribeAllObjects()
    {
        UpdateManager.Instance.UnsubscribeAll();
        activePlayers.Clear();
    }
}

