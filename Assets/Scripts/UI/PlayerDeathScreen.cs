using System.Collections;
using Combat.Stats;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventType = Events.EventType;

namespace UI
{
    public class PlayerDeathScreen : MonoBehaviour
    {
        [SerializeField] private float deathBeatDuration = 1.5f;
        [SerializeField] private GameObject deathScreenUI;

        [Header("Content")]
        [SerializeField] private TMP_Text headingText;
        [SerializeField] private TMP_Text scrapPayout;
        [SerializeField] private TMP_Text scrapFound;
        [SerializeField] private TMP_Text killCount;
        [SerializeField] private TMP_Text exploitCount;
        [SerializeField] private TMP_Text companionOpinion;
        [SerializeField] private StatTracker statTracker;

        private void OnValidate()
        {
            if (statTracker == null)
                statTracker = FindObjectOfType<StatTracker>();
        }

        private void OnEnable()
        {
            EventManager.currentManager.Subscribe(EventType.PlayerDied, OnPlayerDied);
        }

        private void OnDisable()
        {
            EventManager.currentManager.Unsubscribe(EventType.PlayerDied, OnPlayerDied);
        }

        private void OnPlayerDied(EventData eventData)
        {
            if (!eventData.IsEventOfType(out PlayerDied command)) return;
            StartCoroutine(DeathSequence(command.LastKiller));
        }

        private IEnumerator DeathSequence(GameObject killer)
        {
            PlayDeathBeat();
            yield return new WaitForSeconds(deathBeatDuration);

            ShowDeathScreen(killer);
        }

        private void PlayDeathBeat()
        {
            
        }

        private void ShowDeathScreen(GameObject killer)
        {
            deathScreenUI.SetActive(true);

            bool killedByCompanion = killer != null && killer.CompareTag("Companion");
            headingText.text = killedByCompanion ? "Betrayed" : "You Died";

            scrapFound.text = statTracker.ScrapFound.ToString();
            scrapPayout.text = statTracker.ScrapPayout.ToString();
            exploitCount.text = statTracker.TotalExploitedEnemies.ToString();
            killCount.text = statTracker.TotalKills.ToString();

            float grievance = GetCurrentGrievance();
            companionOpinion.text = GrievanceLabelResolver.GetLabel(grievance);
            companionOpinion.color = GrievanceLabelResolver.GetColor(grievance);
        }

        private float GetCurrentGrievance()
        {
            return GameState.CompanionGrievance;
        }

        public void ConfirmReturnToHub()
        {
            GameState.ResetForNewGame();
            DialogueManager.ClearRunState();
            SceneManager.LoadScene("PlayerHub", LoadSceneMode.Single);
        }
    }
}
