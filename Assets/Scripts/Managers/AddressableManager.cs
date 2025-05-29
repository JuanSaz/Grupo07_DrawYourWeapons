using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour
{
    public static AddressableManager Instance { get; private set; }


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

    public void LoadAsset(AssetReference reference)
    {
        //NADIE SABE ESTO GENTE
    }

    //public IEnumerator <GameObject> InstantiateAsset(AssetReference assetReference)
    //{
    //   AsyncOperationHandle<GameObject> opHandle = assetReference.InstantiateAsync();

    //    //if (!opHandle.IsDone) 
    //    //{
            
            
    //    //}
    //    //return opHandle.Result;



    //}
}
