using UnityEngine;


[CreateAssetMenu(fileName = "PlayerBehavior", menuName = "Scriptable Objects/MyBehavior/Player")]
public class PlayerBehavior : MyBehavior
{
    [SerializeField] SO_PlayerInput inputsRef;
    [SerializeField] string shootSoundID;
    public override Entity CreateEntity()
    {
        var player = new PlayerEntity();
        player.playerBehavior = this;
        player.inputs = inputsRef;
        player.shootSoundID = shootSoundID;
        return player;
    }
}
