using NUnit.Framework;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class UpdateManager : MonoBehaviour
{
    private List<IUpdatable> scriptsToUpdate = new List<IUpdatable>();

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

    public void Subscribe(IUpdatable obj)
    {
        scriptsToUpdate.Add(obj);
    }

    public void Unsubscribe(IUpdatable obj)
    {
        int index = scriptsToUpdate.IndexOf(obj);
        if (index >= 0)
        {
            scriptsToUpdate[index] = null;
        }
    }
    
}
