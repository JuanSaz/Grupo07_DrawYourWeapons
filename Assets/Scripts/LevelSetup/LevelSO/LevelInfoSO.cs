using UnityEngine;


[CreateAssetMenu(fileName = "LevelInfo", menuName = "Scriptable Objects/Levels/LevelInfo")]

public class LevelInfoSO : ScriptableObject
{
    public Vector3[] wallPositions;
}
