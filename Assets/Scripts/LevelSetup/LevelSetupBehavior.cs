using UnityEngine;

[CreateAssetMenu(fileName = "LevelSetupBehavior", menuName = "Scriptable Objects/MyBehavior/LevelSetupBehavior")]
public class LevelSetupBehavior : MyBehavior
{
    public LevelInfoSO levelInfo;
    public override Entity CreateEntity()
    {
        var levelEntity = new LevelSetupEntity();
        levelEntity.levelInfo = levelInfo;
        return levelEntity;
    }
}
