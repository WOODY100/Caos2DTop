using UnityEngine;
using UnityEngine.UI;

public class CombatStatusUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject root;
    [SerializeField] private Image fillImage;

    private void Awake()
    {
        if (root != null)
            root.SetActive(false);
    }

    private void OnEnable()
    {
        CombatLockManager.OnCombatStart += HandleCombatStart;
        CombatLockManager.OnCombatEnd += HandleCombatEnd;
    }

    private void OnDisable()
    {
        CombatLockManager.OnCombatStart -= HandleCombatStart;
        CombatLockManager.OnCombatEnd -= HandleCombatEnd;
    }

    private void Update()
    {
        if (CombatLockManager.Instance == null)
            return;

        if (!CombatLockManager.Instance.IsInCombat)
            return;

        if (fillImage != null)
        {
            fillImage.fillAmount =
                CombatLockManager.Instance.CombatNormalizedTime;
        }
    }

    private void HandleCombatStart()
    {
        if (root != null)
            root.SetActive(true);

        if (fillImage != null)
            fillImage.fillAmount = 1f;
    }

    private void HandleCombatEnd()
    {
        if (root != null)
            root.SetActive(false);
    }
}
