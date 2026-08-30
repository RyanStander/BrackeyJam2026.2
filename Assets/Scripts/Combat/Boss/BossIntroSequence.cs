using System.Collections;
using UnityEngine;

namespace Combat.Boss
{
    public class BossIntroSequence : MonoBehaviour
    {
        [SerializeField] private GameObject healthBarRoot;
        [SerializeField] private float introDuration = 2f;

        public IEnumerator PlayIntro()
        {
            healthBarRoot.SetActive(false);
            yield return new WaitForSeconds(introDuration);
            healthBarRoot.SetActive(true);
        }
    }
}
