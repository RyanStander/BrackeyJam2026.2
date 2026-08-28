using System;
using System.Collections;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EventType = Events.EventType;

namespace UI
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        private float displayedCurrentHealth;
        private float realCurrentHealth;

        private float displayedMaxHealth;
        private float realMaxHealth;

        private Coroutine healthTransitionCoroutine;

        private void OnEnable()
        {
            EventManager.currentManager.Subscribe(EventType.UpdatePlayerHealth, OnUpdateHealthDisplay);
            EventManager.currentManager.Subscribe(EventType.SetPlayerHealth, OnSetHealthDisplay);
        }

        private void OnDisable()
        {
            EventManager.currentManager.Unsubscribe(EventType.UpdatePlayerHealth, OnUpdateHealthDisplay);
            EventManager.currentManager.Unsubscribe(EventType.SetPlayerHealth, OnSetHealthDisplay);
        }
        
        private void OnSetHealthDisplay(EventData eventData)
        {
            if (!eventData.IsEventOfType(out SetPlayerHealth setPlayerHealth))
                return;

            realCurrentHealth = setPlayerHealth.CurrentHealth;
            realMaxHealth = setPlayerHealth.MaxHealth;

            displayedCurrentHealth = realCurrentHealth;
            displayedMaxHealth = realMaxHealth;

            healthSlider.maxValue = displayedMaxHealth;
            healthSlider.value = displayedCurrentHealth;
        }

        private void OnUpdateHealthDisplay(EventData eventData)
        {
            if (!eventData.IsEventOfType(out UpdatePlayerHealth updatePlayerHealth))
                return;

            if (healthTransitionCoroutine != null)
            {
                StopCoroutine(healthTransitionCoroutine);
            }
            
            healthTransitionCoroutine = StartCoroutine(SmoothHealthTransition(updatePlayerHealth.CurrentHealth, 0.5f));
        }

        private IEnumerator SmoothHealthTransition(float targetHealth, float duration)
        {
            float startHealth = displayedCurrentHealth;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                displayedCurrentHealth = Mathf.Lerp(startHealth, targetHealth, elapsedTime / duration);
                healthSlider.value = displayedCurrentHealth;
                yield return null;
            }

            displayedCurrentHealth = targetHealth;
            healthSlider.value = displayedCurrentHealth;
        }
    }
}
