using System.Collections;
using Combat.Stats;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private float iFrameDuration = 1f;
    private bool iFrameActive = false;
    private bool isDead = false;
    public override void TakeDamage(float damage)
    {
        if (iFrameActive || isDead)
        {
            return;
        }
        base.TakeDamage(damage);
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