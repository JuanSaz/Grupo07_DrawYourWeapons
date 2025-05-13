using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public List<Transform> spawnPoints = new List<Transform>();

    string PlayerOneName, PlayerTwoName, PlayerThreeName, PlayerFourName;


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
        GameManager.Instance.SetPlayerNames();
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
                    GameManager.Instance.playerPrefabs[0].GetComponent<Player>().PlayerName = PlayerOneName;
                    GameManager.Instance.playerPrefabs[1].GetComponent<Player>().PlayerName = PlayerTwoName;
                    GetPlayers();
                }
                break;
            case 3:
                for (int i = 0; i < GameManager.Instance.playerPrefabs.Count - 1; i++)
                {
                    Vector3 playerSpawnPos = new Vector3(spawnPoints[i].position.x, spawnPoints[i].position.y, 0);
                    Instantiate(GameManager.Instance.playerPrefabs[i], playerSpawnPos, spawnPoints[i].rotation);
                    GameManager.Instance.playerPrefabs[0].GetComponent<Player>().PlayerName = PlayerOneName;
                    GameManager.Instance.playerPrefabs[1].GetComponent<Player>().PlayerName = PlayerTwoName;
                     GameManager.Instance.playerPrefabs[2].GetComponent<Player>().PlayerName = PlayerThreeName;
                    GetPlayers();
                }
                break;
            case 4:
                for (int i = 0; i < GameManager.Instance.playerPrefabs.Count; i++)
                {
                    Vector3 playerSpawnPos = new Vector3(spawnPoints[i].position.x, spawnPoints[i].position.y, 0);
                    Instantiate(GameManager.Instance.playerPrefabs[i], playerSpawnPos, spawnPoints[i].rotation);
                    GameManager.Instance.playerPrefabs[0].GetComponent<Player>().PlayerName = PlayerOneName;
                    GameManager.Instance.playerPrefabs[1].GetComponent<Player>().PlayerName = PlayerTwoName;
                    GameManager.Instance.playerPrefabs[2].GetComponent<Player>().PlayerName = PlayerThreeName;
                    GameManager.Instance.playerPrefabs[3].GetComponent<Player>().PlayerName = PlayerFourName;
                    GetPlayers();
                }
                break;
        }
    }

    public void GetPlayerNames(string one, string two, string three, string four)
    {
        PlayerOneName = one; PlayerTwoName = two; PlayerThreeName = three; PlayerFourName = four;
        
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

