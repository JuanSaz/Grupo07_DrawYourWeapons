using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    string playerOne, playerTwo, playerThree, playerFour;
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

   public void SetPlayerNames()
    {
        LevelManager.Instance.GetPlayerNames(playerOne, playerTwo, playerThree, playerFour);

    }

    public void ReciveNames(string one, string two, string three, string four)
    {
        playerOne = one;
        playerTwo = two;
        playerThree = three;
        playerFour = four;
    }
    

    
}
