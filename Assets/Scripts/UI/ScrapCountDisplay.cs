using System;
using Events;
using TMPro;
using UnityEngine;
using EventType = Events.EventType;

namespace UI
{
    public class ScrapCountDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text scrapCountText;
        
        private void OnEnable()
        {
            EventManager.currentManager.Subscribe(EventType.UpdatePlayerScrapCount, OnUpdateScrapDisplay);
        }
        
        private void OnDisable()
        {
            EventManager.currentManager.Unsubscribe(EventType.UpdatePlayerScrapCount, OnUpdateScrapDisplay);
        }

        private void OnUpdateScrapDisplay(EventData eventData)
        {
            if (!eventData.IsEventOfType(out UpdatePlayerScrapCount updateScrapCount))
                return;
            
            scrapCountText.text = "Scrap: " + updateScrapCount.CurrentScrap;
        }
    }
}
