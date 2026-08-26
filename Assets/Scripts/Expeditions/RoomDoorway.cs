using UnityEngine;

public class RoomDoorway : MonoBehaviour
{
    public enum RoomDoorwayState
    {
        unknown, unexplored, explored, locked
    }
    public Color GetColor(RoomDoorwayState state)
    {
        switch (state)
        {
            case RoomDoorwayState.unexplored: return Color.yellow;
            case RoomDoorwayState.explored: return Color.green;
            case RoomDoorwayState.locked: return Color.red;
            default: return Color.white;
        }
    }

    [SerializeField]
    private RoomDoorwayState currentState = RoomDoorwayState.unknown;

    [SerializeField]
    private BoxCollider exitCollider;

    [SerializeField]
    private BoxCollider blockCollider;

    [SerializeField]
    private GameObject blockVisual;

    private void UpdateDoorway()
    {
        bool is_locked = currentState is RoomDoorwayState.locked;
        blockVisual.SetActive(is_locked);
        blockCollider.enabled = (is_locked);
    }    

    private void FixedUpdate()
    {
        // Change to instead update on event or call from room manager, for now just update.
        UpdateDoorway();
    }

    private void OnDrawGizmos()
    {
        if (!exitCollider || !blockCollider) return;
        Gizmos.color = GetColor(currentState);
        Gizmos.DrawWireCube(exitCollider.bounds.center, exitCollider.bounds.size);
        Gizmos.DrawWireCube(blockCollider.bounds.center, blockCollider.bounds.size);
    }
}
