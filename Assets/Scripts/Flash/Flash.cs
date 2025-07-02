using UnityEngine;

[CreateAssetMenu(fileName = "Flash", menuName = "Scriptable Objects/Flash")]
public class FlashBehaviour : MyBehavior
{
    public override Entity CreateEntity()
    {
        var flash = new FlashEntity();
        return flash;
    }
}
