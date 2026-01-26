using UnityEngine;

public class SceneFadeIn : MonoBehaviour
{
    private void Start()
    {
        if (FadeManager.Instance != null)
            StartCoroutine(FadeManager.Instance.FadeIn());
    }
}
