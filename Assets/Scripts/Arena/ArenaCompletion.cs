using System;
using System.Collections;
using Combat.Stats;
using Events;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventType = Events.EventType;

namespace Arena
{
    public class ArenaCompletion : MonoBehaviour
    {
        [SerializeField] private float cheerDuration = 2f;
        [SerializeField] private GameObject overviewScreenUI;

        [Header("Content")] [SerializeField] private TMP_Text scrapPayout;
        [SerializeField] private TMP_Text scrapFound;
        [SerializeField] private GameObject itemList;
        [SerializeField] private TMP_Text killCount;
        [SerializeField] private TMP_Text exploitCount;
        [SerializeField] private TMP_Text companionOpinion;
        [SerializeField] private StatTracker statTracker;

        private float grievance;

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
            companionOpinion.text = GetCompanionOpinion();
            companionOpinion.color = GetColor();
            //disable player controls
        }

        public void ConfirmReturnToHub()
        {
            SceneManager.LoadScene("PlayerHub", LoadSceneMode.Single);
        }

        private string GetCompanionOpinion()
        {
            CompanionGrievance companionGrievance = FindObjectOfType<CompanionGrievance>();

            if (companionGrievance == null)
                grievance = 100;
            else
                grievance = companionGrievance.Grievance;

            return grievance switch
            {
                < 12.5f => "Content",
                < 25f => "Wary",
                < 37.5f => "Unsure",
                < 50f => "Suspicious",
                < 62.5f => "Resentful",
                < 75f => "Seething",
                < 87.5f => "Furious",
                _ => "Betrayed"
            };
        }
        
        private Color GetColor()
        {
            float t = Mathf.Clamp01(grievance / 100f);
            return Color.Lerp(Color.green, Color.red, t);
        }
    }
}
