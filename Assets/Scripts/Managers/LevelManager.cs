using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public List<Transform> spawnPoints = new List<Transform>();

    [SerializeField] private GameObject WinCanvas;

    public Action<Player> onPlayerKilled;

    private List<Player> activePlayers = new List<Player>();

    private List<Player> totalPlayers = new List<Player>();

    public List<Player> ActivePlayers { get { return activePlayers; } set { activePlayers = value; } }
    public List<Player> TotalPlayers { get { return totalPlayers; } set { totalPlayers = value; } }

    [SerializeField] private int pointsToWin;

    private Coroutine RestartRoundCor;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SpawnPlayers();

        onPlayerKilled += ManagePlayerKilled;

    }

   public void SpawnPlayers()
    {
        switch (GameManager.Instance.GetPlayerAmount())
        {
            case 2:
                for (int i = 0; i < GameManager.Instance.playerPrefabs.Count - 2; i++)
                {
                    Vector3 playerSpawnPos = new Vector3(spawnPoints[i].position.x, spawnPoints[i].position.y, 0);
                    Instantiate(GameManager.Instance.playerPrefabs[i], playerSpawnPos, spawnPoints[i].rotation);

                }
                break;
            case 3:
                for (int i = 0; i < GameManager.Instance.playerPrefabs.Count - 1; i++)
                {
                    Vector3 playerSpawnPos = new Vector3(spawnPoints[i].position.x, spawnPoints[i].position.y, 0);
                    Instantiate(GameManager.Instance.playerPrefabs[i], playerSpawnPos, spawnPoints[i].rotation);
                }
                break;
            case 4:
                for (int i = 0; i < GameManager.Instance.playerPrefabs.Count; i++)
                {
                    Vector3 playerSpawnPos = new Vector3(spawnPoints[i].position.x, spawnPoints[i].position.y, 0);
                    Instantiate(GameManager.Instance.playerPrefabs[i], playerSpawnPos, spawnPoints[i].rotation);
                }
                break;
        }
    }

    public void SetWinner(Player Winner)
    {
        Debug.Log("El ganador es " + Winner.PlayerName.ToUpper() + "!");

        UnsubscribeAllObjects();
        
        WinInfo info = WinCanvas.GetComponent<WinInfo>();

        info.SetWinnerNameAndScore(Winner.PlayerName, Winner.Score);
        WinCanvas.SetActive(true);
    }

    private void ManagePlayerKilled(Player player)
    {

        activePlayers.Remove(player);

        if (activePlayers.Count == 1)
        {
            activePlayers[0].AddPoint();
            GetPointsFromSurvivour(activePlayers[0]);

            RestartRoundCor = StartCoroutine(RestartRoundAfterDelay(2f));
        }
    }

    public void GetPointsFromSurvivour(Player survivour)
    {
        if (survivour.Score == pointsToWin)
        {
            SetWinner(survivour);
        }
    }

    private IEnumerator RestartRoundAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);

        foreach (var player in totalPlayers)
        {
            player.ResetPlayer();
        }
    }


    public void UnsubscribeAllObjects()
    {
        UpdateManager.Instance.UnsubscribeAll();
        activePlayers.Clear();
    }
}

