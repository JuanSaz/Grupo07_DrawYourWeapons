using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public List<ICollidable> ActivePlayersColls { get => activePlayersColls;}
    public List<ICollidable> ActiveBulletsColls { get => activeBulletsColls;}
    public List<ICollidable> ActiveWallsColls { get => activeWallsColls;}

    [SerializeField] private int playerAmount;
    public List<GameObject> playerPrefabs = new List<GameObject>();
    private List<ICollidable> activePlayersColls = new List<ICollidable>();
    private List<ICollidable> activeBulletsColls = new List<ICollidable>();
    private List<ICollidable> activeWallsColls = new List<ICollidable>();


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartCoroutine(MyStart());
    }

    public void SetPlayerAmount(int amount)
    {
        playerAmount = amount;   
    }

    public int GetPlayerAmount()
    {
        return playerAmount;
    }

    private IEnumerator MyStart()
    {
        yield return null;
        InstantiatorManager.Instance.Create(MyBehaviorType.Player1).WakeUp();
        InstantiatorManager.Instance.Create(MyBehaviorType.Player2).WakeUp();
    }

    public void SetPlayerCollidable(ICollidable collidable, bool active)
    {
        if(active)
        {
            activePlayersColls.Add(collidable);
        }
        else
        {
            activePlayersColls.Remove(collidable);
        }
    }
    public void SetBulletCollidable(ICollidable collidable, bool active)
    {
        if (active)
        {
            activeBulletsColls.Add(collidable);
        }
        else
        {
            activeBulletsColls.Remove(collidable);
        }
    }
    public void SetWallCollidable(ICollidable collidable, bool active)
    {
        if (active)
        {
            activeWallsColls.Add(collidable);
        }
        else
        {
            activeWallsColls.Remove(collidable);
        }
    }

}
