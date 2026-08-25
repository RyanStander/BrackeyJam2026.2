using UnityEngine;
using UnityEngine.UI;

public class HubManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField]
    private Button exit;

    public void Start()
    {
        exit.onClick.AddListener(OnExitGame);
    }

    private void OnExitGame()
    {
        //TODO: Implement save files
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif

        Debug.Log("Exit Game Called");
    }
}