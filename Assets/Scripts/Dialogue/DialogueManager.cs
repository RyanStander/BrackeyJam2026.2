using System.Collections;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSONAsset;
    private Story story;
    private bool dialogueActive = false;

    [SerializeField] private GameObject panelDialogue;
    [SerializeField] private TMP_Text txtSpeaker;
    [SerializeField] private TMP_Text txtDialogue;
    [SerializeField] private Transform containerChoice;
    [SerializeField] private GameObject btnChoicePrefab;
    [SerializeField] private GameObject btnContinue;
    [SerializeField] private GameObject expeditionsMenu;
    [SerializeField] private GameObject hubMainMenu;
    [SerializeField] private GameObject nudgeMessage;
    [SerializeField] private float nudgeDuration = 1.5f;
    private bool isNudging = false;

    private bool choicesActive = false;
    private static string savedStoryJson = null;
    private static string savedPendingLine = null;
    private static bool resumePending = false;

    private static void ResetStaticStateOnLaunch()
    {
        savedStoryJson = null;
        savedPendingLine = null;
        resumePending = false;
    }

    public static void ClearRunState()
    {
        ResetStaticStateOnLaunch();
    }

    private void Awake()
    {
        //panelDialogue.SetActive(false);
        //btnContinue.SetActive(false);
    }

    private void Start()
    {
        if (resumePending)
        {
            ResumeAfterMatch();
        }
        else
        {
            GameState.ResetForNewGame();
        }
    }

    public void StartDialogue()
    {
        if (dialogueActive) return;

        if (resumePending)
        {
            ResumeAfterMatch();
            return;
        }

        story = new Story(inkJSONAsset.text);

        story.ObserveVariable("greivance", OnTrustChanged);

        dialogueActive = true;
        panelDialogue.SetActive(true);
        
        ContinueStory();
    }

    public void TryOpenExpeditions()
    {
        if (resumePending)
        {
            if (expeditionsMenu != null) expeditionsMenu.SetActive(true);
            if (hubMainMenu != null) hubMainMenu.SetActive(false);
            btnContinue.SetActive(false);
            return;
        }

        if (!dialogueActive && !isNudging)
        {
            StartCoroutine(NudgeThenTalk());
        }
    }

    private IEnumerator NudgeThenTalk()
    {
        isNudging = true;

        if (nudgeMessage != null)
        {
            nudgeMessage.SetActive(true);
            yield return new WaitForSeconds(nudgeDuration);
            nudgeMessage.SetActive(false);
        }

        isNudging = false;
        StartDialogue();
    }

    private void OnTrustChanged(string varName, object newValue)
    {
        Debug.Log($"[Dialogue] {varName} changed to {newValue}");
        // need to wire this into CompanionGrievance once the mapping is decide
    }

    public void ContinueStory()
    {
        if (!dialogueActive) return;
        if (choicesActive) return;

        if (story.canContinue)
        {
            string line = story.Continue().Trim();

            if (story.currentTags.Contains("gate:fight"))
            {
                TriggerMatch(line);
                return;
            }

            string endingTag = story.currentTags.Find(t => t.StartsWith("ending:"));
            if (endingTag != null)
            {
                GameState.EndingId = endingTag.Substring("ending:".Length);
                GameState.EndingLine = line;
                UnityEngine.SceneManagement.SceneManager.LoadScene("EndingScreen");
                return;
            }

            DisplayLine(line);
        }
        else if (story.currentChoices.Count > 0)
        {
            ShowChoices();
        }
        else
        {
            EndDialogue();
        }
    }

    private void DisplayLine(string line)
    {
        string speaker = "";
        string text = line;

        int colonIndex = line.IndexOf(':');
        if (colonIndex > 0 && colonIndex < 25)
        {
            string possibleSpeaker = line.Substring(0, colonIndex);
            if (!possibleSpeaker.Contains(".") && !possibleSpeaker.Contains("!") && !possibleSpeaker.Contains("?"))
            {
                speaker = possibleSpeaker;
                text = line.Substring(colonIndex + 1).Trim();
            }
        }

        txtSpeaker.gameObject.SetActive(!string.IsNullOrEmpty(speaker));
        txtSpeaker.text = speaker;
        txtDialogue.text = text;
    }

    private void ShowChoices()
    {
        foreach (Transform child in containerChoice)
            Destroy(child.gameObject);

        foreach (Choice choice in story.currentChoices)
        {
            GameObject buttonObj = Instantiate(btnChoicePrefab, containerChoice);
            buttonObj.SetActive(true);

            TMP_Text label = buttonObj.GetComponentInChildren<TMP_Text>();
            if (label != null) label.text = choice.text;

            int choiceIndex = choice.index;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => MakeChoice(choiceIndex));
            choicesActive = story.currentChoices.Count > 0;
        }
    }

    private void MakeChoice(int choiceIndex)
    {
        choicesActive = false;
        story.ChooseChoiceIndex(choiceIndex);

        foreach (Transform child in containerChoice)
            Destroy(child.gameObject);

        ContinueStory();
    }

    private void TriggerMatch(string pendingLine)
    {
        savedStoryJson = story.state.ToJson();
        savedPendingLine = pendingLine;
        resumePending = true;

        dialogueActive = false;
        panelDialogue.SetActive(false);
        btnContinue.SetActive(false);
    }

    private void ResumeAfterMatch()
    {
        resumePending = false;

        story = new Story(inkJSONAsset.text);
        story.state.LoadJson(savedStoryJson);
        story.ObserveVariable("greivance", OnTrustChanged);
        savedStoryJson = null;

        dialogueActive = true;
        panelDialogue.SetActive(true);

        if (!string.IsNullOrEmpty(savedPendingLine))
        {
            DisplayLine(savedPendingLine);
            savedPendingLine = null;
        }
        else
        {
            ContinueStory();
        }
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        story.RemoveVariableObserver(OnTrustChanged, "greivance");
        panelDialogue.SetActive(false);
        hubMainMenu.SetActive(true);
    }

    public void ResetGame()
    {
        GameState.ResetForNewGame();
        ResetStaticStateOnLaunch();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}