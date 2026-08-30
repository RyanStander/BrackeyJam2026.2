using Combat.Stats;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventType = Events.EventType;

namespace Arena
{
    public class ArenaManager : MonoBehaviour
    {
        [SerializeField]
        private ArenaData data;

        [SerializeField]
        private ArenaCombatHelper combatHelper;

        [SerializeField]
        private GameObject playerCharacter, companionCharacter;

        [Tooltip("Only applicable if there is not already a delay with the spawner"), Range(0, 5)]
        private const int arenaStartDelay = 5;

        private void OnEnable()
        {
            EventManager.currentManager.Subscribe(EventType.WavesCompleted, OnWavesCompleted);
            EventManager.currentManager.Subscribe(EventType.ReturnToHub, OnReturnToHub);
        }

        private void OnDisable()
        {
            EventManager.currentManager.Unsubscribe(EventType.WavesCompleted, OnWavesCompleted);
            EventManager.currentManager.Unsubscribe(EventType.ReturnToHub, OnReturnToHub);
        }

        private void OnRunWaves()
        {
            combatHelper.RunHelper(data.Waves, data.WaveDelay);
        }

        private void Start()
        {
            Invoke(nameof(OnRunWaves), data.WaveDelay != 0 ? 0 : arenaStartDelay);
        }

        private void OnWavesCompleted(EventData eventData)
        {
            if (!eventData.IsEventOfType(out WavesCompleted command)) return;
            Debug.Log("All Waves Cleared!");
        }

        private void OnReturnToHub(EventData eventData)
        {
            // TODO: The player hub is not yet configured to the new arena, return for now.
            return;

            if (!eventData.IsEventOfType(out ReturnToHub command)) return;
            // TODO: Handle any needed cleanup or end-steps for Arenas, for now instant return to main menu.

            // TODO: Create scene manager to handle transitions
            Debug.Log("OnReturnToHub Requested (Loading Scene 'PlayerHub')...");
            SceneManager.LoadScene("PlayerHub", LoadSceneMode.Single);
        }

        private void Update()
        {
            // Testing function to kill everything (excl. player & companion)
            if (Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                Health[] healths = FindObjectsOfType<Health>();
                foreach (Health health in healths)
                {
                    if (health.gameObject.CompareTag("Player") || health.gameObject.CompareTag("Companion")) continue;
                    health.TakeDamage(new Combat.Data.DamageInfo(50f, Combat.Data.Faction.Allies, null));
                }
            }
        }
    }
}
