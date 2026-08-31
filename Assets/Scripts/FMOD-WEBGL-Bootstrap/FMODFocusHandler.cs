using FMODUnity;
using UnityEngine;

namespace FMOD_WEBGL_Bootstrap
{
    public class FMODFocusHandler : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
                RuntimeManager.CoreSystem.mixerResume();
            else
                RuntimeManager.CoreSystem.mixerSuspend();
        }
    }
}
