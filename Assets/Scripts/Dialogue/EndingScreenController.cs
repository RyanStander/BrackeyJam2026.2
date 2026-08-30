using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScreenController : MonoBehaviour
{
    [SerializeField] private TMP_Text endingText;
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float fadeDuration = 2f;

    private void Start()
    {
        endingText.text = GameState.EndingLine;
        fadeGroup.alpha = 0f;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = t / fadeDuration;
            yield return null;
        }
        fadeGroup.alpha = 1f;
    }

    public void ResetGame()
    {
        GameState.ResetForNewGame();
        DialogueManager.ClearRunState();
        SceneManager.LoadScene("PlayerHub");
    }
}
