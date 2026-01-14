using UnityEngine;

public class LoadTester : MonoBehaviour
{
    void Start()
    {
        SaveManager.Instance.SaveGameToSlot(0);
        SaveManager.Instance.SaveGameToSlot(1);
    }
}