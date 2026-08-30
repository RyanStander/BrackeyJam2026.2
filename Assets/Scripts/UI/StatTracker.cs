using System;
using System.Collections.Generic;
using Events;
using UnityEngine;
using EventType = Events.EventType;

namespace UI
{
    public class StatTracker : MonoBehaviour
    {
        public int ScrapPayout { get; private set; }

        public int ScrapFound { get; private set; }

        //public List<Item> ItemsFound { get; private set; }
        public int TotalKills { get; private set; }
        public int TotalExploitedEnemies { get; private set; }
        public float CurrentCompanionGrievance { get; private set; }

        private void OnEnable()
        {
            EventManager.currentManager.Subscribe(EventType.ScrapPickedUp, AddScrap);
            EventManager.currentManager.Subscribe(EventType.OnEnemyDeath, AddKill);
            EventManager.currentManager.Subscribe(EventType.EnemyExploited, AddExploitedEnemy);
            EventManager.currentManager.Subscribe(EventType.ScrapPayout, AddScrapPayout);
        }

        private void OnDisable()
        {
            EventManager.currentManager.Unsubscribe(EventType.ScrapPickedUp, AddScrap);
            EventManager.currentManager.Unsubscribe(EventType.OnEnemyDeath, AddKill);
            EventManager.currentManager.Unsubscribe(EventType.EnemyExploited, AddExploitedEnemy);
            EventManager.currentManager.Unsubscribe(EventType.ScrapPayout, AddScrapPayout);
        }

        private void AddScrap(EventData eventData)
        {
            if (!eventData.IsEventOfType(out ScrapPickedUp scrapPickedUp))
                return;

            ScrapFound += scrapPickedUp.ScrapCount;
        }

        private void AddScrapPayout(EventData eventData)
        {
            if (!eventData.IsEventOfType(out ScrapPayout scrapPayout))
                return;

            ScrapPayout += scrapPayout.ScrapCount;
        }

        private void AddKill(EventData eventData)
        {
            if (!eventData.IsEventOfType(out OnEnemyDeath onEnemyDeath))
                return;

            TotalKills++;
        }

        private void AddExploitedEnemy(EventData eventData)
        {
            if (!eventData.IsEventOfType(out EnemyExploited enemyExploited))
                return;

            TotalExploitedEnemies++;
        }
    }
}
