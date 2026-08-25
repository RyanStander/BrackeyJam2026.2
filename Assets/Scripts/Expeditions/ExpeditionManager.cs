using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HubNavigation : MonoBehaviour
{
    [SerializeField]
    private Button prototype_AreaButton;

    public void Start()
    {
        prototype_AreaButton.onClick.AddListener(Prototype_OnLoadExampleArea);
    }

    private void Prototype_OnLoadExampleArea()
    {
        SceneManager.LoadScene("Expedition", LoadSceneMode.Single);
    }
}