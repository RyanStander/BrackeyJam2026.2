using AudioManagement;
using AudioManagement.SoundLibraries;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Simplified to single pre-made routes for interim
public class HubExpeditionManager : MonoBehaviour
{
    [SerializeField]
    private RegionData activeRegion;

    [SerializeField]
    private Button[] prototypeAreaButtons;

    [SerializeField]
    private TMP_Text region, destination, stickyNote;

    // Make asynch with fade to black, loading symbol and then completion
    private void OnAreaSelected(AreaData area)
    {
        Debug.Log("OnAreaSelected Requested: " + area.TargetScene + " (Loading Prototype Scene 'Expedition')...");
        SceneManager.LoadScene(area.TargetScene, LoadSceneMode.Single); //area.TargetScene
    }

    // Currently only changes data display
    private void UpdateRegionDisplay()
    {
        region.text = "Region: " + activeRegion.RegionName;
        destination.text = "Destination: " + activeRegion.RegionDestination;
        stickyNote.text = activeRegion.StickyNoteText;

        if (GameState.CurrentArea >= activeRegion.AreasLinear.Length)
        {
            //TODO: Tell player that they are done with the game and need to reset.
            return;
        }

        // TODO: Separate this into its own manager and implement branching, for now we assume a linear path.
        for (int i = 0; i < activeRegion.AreasLinear.Length; i++)
        {
            bool isCurrent = (i == GameState.CurrentArea);//0:true|1:false|2:false|3:false
            bool isPast = (i < GameState.CurrentArea);//0:false|1:false|2:false|3:false
            bool isNext = (i == GameState.CurrentArea + 1);//0:false|1:true|2:false|3:false
            bool isKnown = isCurrent || isPast || isNext;//0:true|1:true|2:false|3:false

            prototypeAreaButtons[i].enabled = true;
            prototypeAreaButtons[i].interactable = isCurrent;
            prototypeAreaButtons[i].image.sprite = activeRegion.Icons.GetMapIcon(isCurrent, isPast, isNext);

            int index = i; // Scope requirement for listener below
            prototypeAreaButtons[i].onClick.AddListener(() => OnAreaSelected(activeRegion.AreasLinear[index]));
        }
    }

    public void Start()
    {
        if (!activeRegion) return;
        UpdateRegionDisplay();
        
        AudioManager.PlayMusic(AudioDataHandler.Music.HUB);
    }
}
