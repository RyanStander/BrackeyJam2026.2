using System.Collections;
using Combat.Stats;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventType = Events.EventType;

namespace UI
{
    public class ArenaCompletion : MonoBehaviour
    {
        [SerializeField] private float cheerDuration = 2f;
        [SerializeField] private GameObject overviewScreenUI;

        [Header("Content")]
        [SerializeField] private TMP_Text scrapPayout;
        [SerializeField] private TMP_Text scrapFound;
        [SerializeField] private GameObject itemList;
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
            EventManager.currentManager.Subscribe(EventType.WavesCompleted, OnWavesCompleted);
        }

        private void OnDisable()
        {
            EventManager.currentManager.Unsubscribe(EventType.WavesCompleted, OnWavesCompleted);
        }

        private void OnWavesCompleted(EventData eventData)
        {
            if (!eventData.IsEventOfType(out WavesCompleted command)) return;
            StartCoroutine(CompletionSequence());
        }

        private IEnumerator CompletionSequence()
        {
            PlayCrowdCheer();
            yield return new WaitForSeconds(cheerDuration);

            ShowOverviewScreen();
        }

        private void PlayCrowdCheer()
        {
            // add audio
            // crowd throws scrap and other junk into the arena
        }

        private void ShowOverviewScreen()
        {
            overviewScreenUI.SetActive(true);

            scrapFound.text = statTracker.ScrapFound.ToString();
            scrapPayout.text = statTracker.ScrapPayout.ToString();
            exploitCount.text = statTracker.TotalExploitedEnemies.ToString();
            killCount.text = statTracker.TotalKills.ToString();

            float grievance = GetCurrentGrievance();
            companionOpinion.text = GrievanceLabelResolver.GetLabel(grievance);
            companionOpinion.color = GrievanceLabelResolver.GetColor(grievance);

            //disable player controls
        }

        private float GetCurrentGrievance()
        {
            return GameState.CompanionGrievance;
        }

        public void ConfirmReturnToHub()
        {
            SceneManager.LoadScene("PlayerHub", LoadSceneMode.Single);
        }
    }
}
