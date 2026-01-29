using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchFlicker : MonoBehaviour
{
    public Light2D light2D;
    public float minIntensity = 0.9f;
    public float maxIntensity = 1.2f;
    public float flickerSpeed = 8f;

    void Awake()
    {
        if (!light2D)
            light2D = GetComponent<Light2D>();

        if (!light2D)
        {
            Debug.LogError(
                $"[TorchFlicker] No Light2D found on {gameObject.name}",
                this
            );
            enabled = false; // 🔒 evita el crash
        }
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}
