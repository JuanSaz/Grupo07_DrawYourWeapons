using UnityEngine;


[CreateAssetMenu(fileName = "PlayerBehavior", menuName = "Scriptable Objects/MyBehavior/Player")]
public class PlayerBehavior : MyBehavior
{
    [SerializeField] SO_PlayerInput inputsRef;
    public override Entity CreateEntity()
    {
        var player = new PlayerEntity();
        player.inputs = inputsRef;
        return player;
    }
}
