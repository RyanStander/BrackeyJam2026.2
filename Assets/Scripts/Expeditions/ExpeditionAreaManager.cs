using Events;
using Expeditions.Room;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventType = Events.EventType;

namespace Expeditions
{
    public class ExpeditionAreaManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject playerCharacter;

        [SerializeField]
        private RoomManager currentRoom;

        private RoomManager[] areaRooms;

        private void OnEnable()
        {
            areaRooms = FindObjectsOfType<RoomManager>();
            playerCharacter = GameObject.FindGameObjectWithTag("Player");
            currentRoom.UpdateRoom(RoomState.explored);

            EventManager.currentManager.Subscribe(EventType.CommandAreaExit, OnCommandAreaExit);
            EventManager.currentManager.Subscribe(EventType.CommandRoomChange, OnCommandRoomChange);
        }

        private void OnDisable()
        {
            EventManager.currentManager.Unsubscribe(EventType.CommandAreaExit, OnCommandAreaExit);
            EventManager.currentManager.Unsubscribe(EventType.CommandRoomChange, OnCommandRoomChange);
        }

        private void OnCommandAreaExit(EventData eventData)
        {
            if (!eventData.IsEventOfType(out CommandAreaExit command)) return;
            // TODO: Handle any needed cleanup or end-steps for expedition, for now instant return to main menu.

            // TODO: Create scene manager to handle transitions
            Debug.Log("OnAreaSelected Requested (Loading Scene 'PlayerHub')...");
            SceneManager.LoadScene("PlayerHub", LoadSceneMode.Single);
        }

        private void OnCommandRoomChange(EventData eventData)
        {
            // TODO: Add some error'ing
            if (!eventData.IsEventOfType(out CommandRoomChange command)) return;
            if (currentRoom != command.SourceRoom) return;

            // TODO: This should not be needed here once proper room progression/exploration is done.
            //currentRoom.UpdateRoom(RoomState.explored); // Mark as explored/complete
            currentRoom = command.TargetRoom;

            #region Position Update
            playerCharacter.GetComponent<CharacterController>().enabled = false;
            Vector3 position = command.TargetPosition;
            position.y = playerCharacter.transform.position.y;
            playerCharacter.transform.position = position;
            playerCharacter.GetComponent<CharacterController>().enabled = true;
            #endregion

            // TODO: Create fade-out sequence before completing teleport, for now instant teleport.

            // TODO: Make room start generating objective based on room data (if applicable).
            // currentRoom.initializeObjective()

            currentRoom.OnRoomEnter();
        }

        private void FixedUpdate()
        {
            // When possible update only on condition changes, for now/simplicity continuous update
            foreach (RoomManager room in areaRooms)
            {
                room.UpdateRoom();
            }
        }

    #if UNITY_EDITOR
        private void OnValidate()
        {
            if (!playerCharacter)
            {
                playerCharacter = GameObject.FindGameObjectWithTag("Player");
            }
        }
    #endif
    }
}