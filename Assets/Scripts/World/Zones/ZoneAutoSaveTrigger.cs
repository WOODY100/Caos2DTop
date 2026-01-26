using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ZoneAutoSaveTrigger : MonoBehaviour
{
    [Header("Autosave")]
    [SerializeField] private bool autoSaveOnEnter = true;

    [Tooltip("Opcional: identificador para logs y debugging")]
    [SerializeField] private string zoneID;

    private bool triggered;

    private void Reset()
    {
        // Asegurar que el collider sea trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!autoSaveOnEnter) return;
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        AutoSaveManager.Instance?.TryAutoSave(
            string.IsNullOrEmpty(zoneID)
                ? "ZoneEnter"
                : $"Zone:{zoneID}"
        );
    }
}
