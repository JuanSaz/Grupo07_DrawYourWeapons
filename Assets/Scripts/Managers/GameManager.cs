using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public string currentScene;
    public static GameManager Instance { get; private set; }
    public List<ICollidable> ActivePlayersColls { get => activePlayersColls; }
    public List<ICollidable> ActiveBulletsColls { get => activeBulletsColls; }
    public List<ICollidable> ActiveWallsColls { get => activeWallsColls; }
    public List<ICollidable> ActivePowerUpColls { get => activePowerUpColls; }

    public List<ICollidable> ActiveDrawSegments { get => activeDrawSegments; }

    [SerializeField] private int playerAmount;
    public List<GameObject> playerPrefabs = new List<GameObject>();
    private List<ICollidable> activePlayersColls = new List<ICollidable>();
    private List<ICollidable> activeBulletsColls = new List<ICollidable>();
    private List<ICollidable> activeWallsColls = new List<ICollidable>();
    private List<ICollidable> activeDrawSegments = new List<ICollidable>();
    private List<ICollidable> activePowerUpColls = new List<ICollidable>();

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

        currentScene = SceneManager.GetActiveScene().name;
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
        InstantiatorManager.Instance.PreloadAddressables();
    }
    public void SetPlayerCollidable(ICollidable collidable, bool active)
    {
        if (active)
        {
            activePlayersColls.Add(collidable);
        }
        else
        {
            int index = activePlayersColls.IndexOf(collidable);

            if (index >= 0)
            {
                activePlayersColls[index] = null;
            }
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
            int index = activeBulletsColls.IndexOf(collidable);

            if (index >= 0)
            {
                activeBulletsColls[index] = null;
            }
        }
    }

    public void SetPowerUpCollidable(ICollidable collidable, bool active)
    {
        if (active)
        {
            activePowerUpColls.Add(collidable);
        }
        else
        {
            int index = activePowerUpColls.IndexOf(collidable);

            if (index >= 0)
            {
                activePowerUpColls[index] = null;
            }
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
            int index = activeWallsColls.IndexOf(collidable);

            if (index >= 0)
            {
                activeWallsColls[index] = null;
            }
        }
    }
    
    public void DeactivateAllCollisions()//AGREGAR A LOS DRAWINGS
    {
        for (int i = 0; i < activeBulletsColls.Count; i++)
        {
            activeBulletsColls[i] = null;
        }
        for (int i = 0; i < activePlayersColls.Count; i++)
        {
            activePlayersColls[i] = null;
        }
        for (int i = 0; i < activeWallsColls.Count; i++)
        {
            activeWallsColls[i] = null;
        }
        for (int i = 0; i < activeDrawSegments.Count; i++)
        {
            activeDrawSegments[i] = null;
        }
        for (int i = 0; i < activePowerUpColls.Count; i++)
        {
            activePowerUpColls[i] = null;
        }
    }
    
    public void SetDrawingCollidable(ICollidable collidable, bool active)
    {
        if (active)
        {
            activeDrawSegments.Add(collidable);
        }
        else
        {
            int index = activeDrawSegments.IndexOf(collidable);

            if (index >= 0)
            {
                activeDrawSegments[index] = null;
            }
        }
    }

}
