using UnityEngine;

public class FlashEntity: Entity, IUpdatable
{
    private float lifeSpan = 0.15f;
    private float timer = 0f;

    public override void WakeUp()
    {
        base.WakeUp();
    }

    public void UpdateMe(float deltaTime)
    {
        Appear();
    }

    private void Appear()
    {
        if (timer < lifeSpan)
        {
            timer += Time.deltaTime;
            if (timer > lifeSpan)
            {
                timer = 0;
                Reset();
            }
        }
    }

    public void Reset()
    {
        UpdateManager.Instance.Unsubscribe(this);
        LevelManager.Instance.OnRoundRestart.RemoveListener(Reset);

        timer = 0;
        EntityGameObject.transform.position = new Vector3(0, 0, 0);

        InstantiatorManager.Instance.flashPool.pool.Release(this);
    }

}
