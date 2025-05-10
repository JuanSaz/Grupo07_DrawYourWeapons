using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public List<Transform> spawnPoints = new List<Transform>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SpawnPlayers();
    }

    void SpawnPlayers()
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
}

