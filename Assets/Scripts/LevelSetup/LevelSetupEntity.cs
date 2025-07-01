using System;
using UnityEngine;


public class LevelSetupEntity : Entity
{
    public LevelInfoSO levelInfo;

    public override void WakeUp()
    {
        base.WakeUp();
        InstantiateWalls();
    }

    private void InstantiateWalls()
    {
        for (int i = 0; i < levelInfo.wallPositions.Length; i++)
        {
            Entity wallEntity = InstantiatorManager.Instance.Create(MyBehaviorType.Wall);
            wallEntity.EntityGameObject.transform.position = levelInfo.wallPositions[i];

            Vector3 currentEuler = wallEntity.EntityGameObject.transform.rotation.eulerAngles;
            wallEntity.EntityGameObject.transform.rotation = Quaternion.Euler(currentEuler.x, currentEuler.y, levelInfo.wallPositions[i].z);
            wallEntity.WakeUp();
        }
    }
}
