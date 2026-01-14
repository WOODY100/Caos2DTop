using UnityEngine;

public class SaveMenuController : MonoBehaviour
{
    [Header("UI")]
    public Transform content;
    public SaveSlotUI slotPrefab;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        var saves = SaveManager.Instance.GetAllSaves();

        for (int slot = 0; slot < SaveManager.MaxSlots; slot++)
        {
            SaveSlotUI ui = Instantiate(slotPrefab, content);

            var save = saves.Find(s => s.slot == slot);

            if (save.meta != null)
            {
                ui.SetupOccupied(slot, save.meta);
            }
            else
            {
                ui.SetupEmpty(slot);
            }
        }
    }
}
