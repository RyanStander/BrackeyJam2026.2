using Combat.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BossHealthBarUI : MonoBehaviour
    {
        [SerializeField] private Health bossHealth;
        [SerializeField] private Image fillImage;

        private void Update()
        {
            if (bossHealth == null) return;
            fillImage.fillAmount = bossHealth.CurrentHealth / bossHealth.MaxHealth;
        }
    }
}
