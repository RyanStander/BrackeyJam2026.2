using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Simplified to single pre-made routes for interim
public class HubExpeditionManager : MonoBehaviour
{
    [SerializeField]
    private RegionData activeRegion;

    // Currently no progression
    [SerializeField][Range(0, 4)]
    private int currentArea = 0;

    [SerializeField]
    private Button[] prototype_AreaButtons;

    [SerializeField]
    private TMP_Text region, destination, stickyNote;

    // Make asynch with fade to black, loading symbol and then completion
    private void OnAreaSelected(AreaData area)
    {
        Debug.Log("OnAreaSelected Requested: " + area.TargetScene + " (Loading Prototype Scene 'Expedition')...");
        SceneManager.LoadScene("Expedition", LoadSceneMode.Single); //area.TargetScene
    }

    // Currently only changes data display
    private void UpdateRegionDisplay()
    {
        region.text = "Region: " + activeRegion.RegionName;
        destination.text = "Destination: " + activeRegion.RegionDestination;
        stickyNote.text = activeRegion.StickyNoteText;

        // TODO: Separate this into its own manager and implement branching, for now we assume a linear path.
        for (int i = 0; i < activeRegion.AreasLinear.Length; i++)
        {
            bool is_current = (i == currentArea);
            bool is_past = (i < currentArea);
            bool is_next = (i == currentArea + 1);
            bool is_known = is_current || is_past || is_next;

            prototype_AreaButtons[i].enabled = is_next || !is_known;
            prototype_AreaButtons[i].interactable = is_next;
            prototype_AreaButtons[i].image.sprite = activeRegion.Icons.GetMapIcon(is_current, is_past, is_next);

            int index = i; // Scope requirement for listener below
            prototype_AreaButtons[i].onClick.AddListener(() => OnAreaSelected(activeRegion.AreasLinear[index]));
        }
    }

    public void Start()
    {
        if (!activeRegion) return;
        UpdateRegionDisplay();
    }
}