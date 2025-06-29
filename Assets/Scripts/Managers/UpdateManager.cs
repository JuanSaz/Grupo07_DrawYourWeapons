using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class UpdateManager : MonoBehaviour
{
    private List<IUpdatable> scriptsToUpdate = new List<IUpdatable>();
    private List<IFixUpdatable> scriptsToFixUpdate = new List<IFixUpdatable>();

    public static UpdateManager Instance { get; private set; }

    void Awake()
    {
        //SINGLETON
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < scriptsToUpdate.Count; i++)
        {
            if (scriptsToUpdate[i] == null) continue;
            scriptsToUpdate[i].UpdateMe(Time.deltaTime); //Updatea los scripts
        }
        //Remueve a todos los que ya no están subscriptos
        scriptsToUpdate.RemoveAll(obj => obj == null);
    }
    private void FixedUpdate()
    {
        for (int i = 0; i < scriptsToFixUpdate.Count; i++)
        {
            if (scriptsToFixUpdate[i] == null) continue;
            scriptsToFixUpdate[i].FixUpdateMe(); //Updatea los scripts
        }
        //Remueve a todos los que ya no están subscriptos
        scriptsToFixUpdate.RemoveAll(obj => obj == null);
        GameManager.Instance.ActivePlayersColls.RemoveAll(obj => obj == null);
        GameManager.Instance.ActiveBulletsColls.RemoveAll(obj => obj == null);
        GameManager.Instance.ActiveWallsColls.RemoveAll(obj => obj == null);
    }
    public void Subscribe(IUpdatable obj)
    {
        scriptsToUpdate.Add(obj);
    }
    public void FixSubscribe(IFixUpdatable obj)
    {
        scriptsToFixUpdate.Add(obj);
    }

    public void UnsubscribeAll()
    {
        for (int i = 0; i < scriptsToUpdate.Count; i++)
        {
            scriptsToUpdate[i] = null;
        }
    }
    public void FixUnsubscribeAll()
    {
        for (int i = 0; i < scriptsToFixUpdate.Count; i++)
        {
            scriptsToFixUpdate[i] = null;
        }
    }

    public void Unsubscribe(IUpdatable obj)
    {
        int index = scriptsToUpdate.IndexOf(obj);

        if (index >= 0)
        {
            scriptsToUpdate[index] = null;
        }
    }
    public void FixUnsubscribe(IFixUpdatable obj)
    {
        int index = scriptsToFixUpdate.IndexOf(obj);

        if (index >= 0)
        {
            scriptsToFixUpdate[index] = null;
        }
    }
}
