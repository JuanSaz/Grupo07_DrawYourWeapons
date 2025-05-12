using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private int playerAmount;
    public List<GameObject> playerPrefabs = new List<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerAmount(int amount)
    {
        playerAmount = amount;   
    }

    public int GetPlayerAmount()
    {
        return playerAmount;
    }

    public void DestroyParty()
    {
        foreach(var player in LevelManager.Instance.ActivePlayers)
        {
            Destroy(player);
        }
    }
}
