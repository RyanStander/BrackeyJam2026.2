using System.Collections;
using Combat.Data;
using Combat.Stats;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private float iFrameDuration = 1f;
    private bool iFrameActive = false;

    public override void TakeDamage(DamageInfo damageInfo)
    {
        if (iFrameActive || isDead)
        {
            return;
        }
        base.TakeDamage(damageInfo);
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