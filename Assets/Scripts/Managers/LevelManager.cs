using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public List<Transform> spawnPoints = new List<Transform>();

    [SerializeField] private GameObject WinCanvas;

    private List<Player> activePlayers;

    public List<Player> ActivePlayers { get { return activePlayers; } set { activePlayers = value; } }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        SpawnPlayers();
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

                    GetPlayers();
                }
                break;
            case 3:
                for (int i = 0; i < GameManager.Instance.playerPrefabs.Count - 1; i++)
                {
                    Vector3 playerSpawnPos = new Vector3(spawnPoints[i].position.x, spawnPoints[i].position.y, 0);
                    Instantiate(GameManager.Instance.playerPrefabs[i], playerSpawnPos, spawnPoints[i].rotation);
                    GetPlayers();
                }
                break;
            case 4:
                for (int i = 0; i < GameManager.Instance.playerPrefabs.Count; i++)
                {
                    Vector3 playerSpawnPos = new Vector3(spawnPoints[i].position.x, spawnPoints[i].position.y, 0);
                    Instantiate(GameManager.Instance.playerPrefabs[i], playerSpawnPos, spawnPoints[i].rotation);
                    GetPlayers();
                }
                break;
        }
    }

    public void SetWinner(Player Winner)
    {
        Debug.Log("El ganador es " + Winner.PlayerName.ToUpper() + "!");

        DestroyParty();
        
        WinInfo info = WinCanvas.GetComponent<WinInfo>();

        info.SetWinnerNameAndScore(Winner.PlayerName, Winner.Score);
        WinCanvas.SetActive(true);

    }

    private void GetPlayers()
    {
        ActivePlayers = FindObjectsOfType<Player>().ToList();
    }

    public void DestroyParty()
    {
        foreach (var player in activePlayers)
        {
            UpdateManager.Instance.Unsubscribe(player);
        }
        activePlayers.Clear();
    }
}

