using Player;

namespace Combat
{
    using Controllers;
    using Movement;
    using UnityEngine;

    public static class KnockbackHelper
    {
        public static void Apply(GameObject target, Vector3 direction, float force, float duration = 0.3f)
        {
            if (target.TryGetComponent(out AIController targetAI) && targetAI.IsKnockbackImmune)
                return;

            if (target.TryGetComponent(out AiMovement aiMovement))
                aiMovement.BeginManualOverride();
            else if (target.TryGetComponent(out Player.PlayerMovement playerMovement))
                playerMovement.ApplyKnockback(duration);

            if (target.TryGetComponent(out Rigidbody rb))
                rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        }
    }
}
