using System;
using Events;
using UnityEngine;
using EventType = Events.EventType;

namespace Factories
{
    public class FactoryController : MonoBehaviour
    {
        public static FactoryController Instance { get; private set; }

        [SerializeField] private PickupFactory pickupFactory;
        [SerializeField] private EnemyFactory enemyFactory;

        private void OnValidate()
        {
            if (pickupFactory == null)
                pickupFactory = GetComponent<PickupFactory>();

            if (enemyFactory == null)
                enemyFactory = GetComponent<EnemyFactory>();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void OnEnable()
        {
            EventManager.currentManager.Subscribe(EventType.CreatePickup, OnCreatePickup);
            EventManager.currentManager.Subscribe(EventType.CreateEnemy, OnCreateEnemy);
        }

        private void OnDisable()
        {
            EventManager.currentManager.Unsubscribe(EventType.CreatePickup, OnCreatePickup);
            EventManager.currentManager.Unsubscribe(EventType.CreateEnemy, OnCreateEnemy);
        }

        private void OnCreatePickup(EventData eventData)
        {
            if (!eventData.IsEventOfType(out CreatePickup createPickup))
                return;
            pickupFactory.CreatePickup(createPickup.PickupType, createPickup.Position);
        }

        private void OnCreateEnemy(EventData eventData)
        {
            if (!eventData.IsEventOfType(out CreateEnemy createEnemy))
                return;
            enemyFactory.CreateEnemy(createEnemy.EnemyType, createEnemy.Position);
        }
    }
}
