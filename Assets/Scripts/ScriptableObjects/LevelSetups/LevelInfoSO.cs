using UnityEngine;


[CreateAssetMenu(fileName = "LevelInfoSO", menuName = "Scriptable Objects/Levels/LevelInfoSO")]

public class LevelInfoSO : ScriptableObject
{
    public Vector3[] wallPositions;
}
