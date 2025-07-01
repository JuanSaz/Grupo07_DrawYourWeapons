using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;
using UnityEngine.Events;

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
        SpawnPlayers();

        InstantiatorManager.Instance.Create(MyBehaviorType.LevelSetup).WakeUp();    //Instancia y corre WakeUp del Level Setup
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

    public void SetWinner(PlayerEntity Winner)
    {
        UnsubscribeAllObjects();
        
        WinInfo info = WinCanvas.GetComponent<WinInfo>();

        info.SetWinnerNameAndScore(Winner.PlayerName, Winner.Score);
        WinCanvas.SetActive(true);
    }

    private void ManagePlayerKilled(PlayerEntity player)
    {
        if(player == null)
        {
            Debug.Log("SOY NULAZO");
        }


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

        OnRoundRestart.Invoke();
    }


    public void UnsubscribeAllObjects()
    {
        UpdateManager.Instance.UnsubscribeAll();
        activePlayers.Clear();
    }
}

