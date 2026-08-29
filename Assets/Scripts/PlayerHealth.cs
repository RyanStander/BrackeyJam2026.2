using System.Collections;
using Combat.Data;
using Combat.Stats;
using Events;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private float iFrameDuration = 1f;
    private bool iFrameActive = false;

    protected override void Awake()
    {
        base.Awake();
        EventManager.currentManager.AddEvent(new SetPlayerHealth(currentHealth, maxHealth));
    }

    public override void TakeDamage(DamageInfo damageInfo)
    {
        if (iFrameActive || isDead)
        {
            return;
        }
        base.TakeDamage(damageInfo);
        EventManager.currentManager.AddEvent(new UpdatePlayerHealth(currentHealth));

        if (isDead)
            EventManager.currentManager.AddEvent(new PlayerDied(LastKiller));
    }

    public void TriggerIFrames()
    {
        TriggerIFrames(iFrameDuration);
    }

    public void TriggerIFrames(float duration)
    {
        StartCoroutine(IFrameRoutine(duration));
    }

    private IEnumerator IFrameRoutine(float duration)
    {
        iFrameActive = true;
        yield return new WaitForSeconds(duration);
        iFrameActive = false;
    }
}
