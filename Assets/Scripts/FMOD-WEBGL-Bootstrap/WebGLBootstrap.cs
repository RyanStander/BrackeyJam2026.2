using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FMOD_WEBGL_Bootstrap
{
    public class WebGLBootstrap : MonoBehaviour
    {
        [SerializeField] private string nextSceneName = "PlayerHub";
        [SerializeField] private CanvasGroup promptGroup;
        [SerializeField] private CanvasGroup fadeGroup;
        [SerializeField] private float promptFadeDuration = 0.4f;
        [SerializeField] private float fadeDuration = 0.5f;

        private AsyncOperation sceneLoad;
        private bool readyToStart;

        private void Awake()
        {
            DontDestroyOnLoad(fadeGroup.transform.root.gameObject);
        }

        private void Start()
        {
            promptGroup.alpha = 0f;
            promptGroup.gameObject.SetActive(false);
            fadeGroup.alpha = 1f;

            StartCoroutine(LoadNextSceneInBackground());
        }

        private IEnumerator LoadNextSceneInBackground()
        {
            sceneLoad = SceneManager.LoadSceneAsync(nextSceneName);
            sceneLoad.allowSceneActivation = false;

            while (sceneLoad.progress < 0.9f)
                yield return null;

            while (!RuntimeManager.HaveAllBanksLoaded)
                yield return null;

            promptGroup.gameObject.SetActive(true);
            yield return StartCoroutine(FadeCanvasGroup(promptGroup, 0f, 1f, promptFadeDuration));

            readyToStart = true;
        }

        private void Update()
        {
            if (!readyToStart) return;

            if (Input.anyKeyDown)
            {
                readyToStart = false;
                StartCoroutine(FadeAndActivate());
            }
        }

        private IEnumerator FadeAndActivate()
        {
            promptGroup.gameObject.SetActive(false);

            sceneLoad.allowSceneActivation = true;
            yield return new WaitUntil(() => sceneLoad.isDone);
        
            yield return null;
            yield return null;
            yield return new WaitForEndOfFrame();

            yield return StartCoroutine(FadeCanvasGroup(fadeGroup, 1f, 0f, fadeDuration));

            Destroy(fadeGroup.transform.root.gameObject);
        }

        private IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                group.alpha = Mathf.Lerp(from, to, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            group.alpha = to;
        }
    }
}
