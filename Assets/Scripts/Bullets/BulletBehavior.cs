using UnityEngine;
[CreateAssetMenu(fileName = "BulletBehavior", menuName = "Scriptable Objects/MyBehavior/Bullet")]
public class BulletBehavior : MyBehavior
{
    public override Entity CreateEntity()
    {
        var bullet = new BulletEntity();
        return bullet;
    }
}
