using UnityEngine;

public class SaveTester : MonoBehaviour
{
    void Start()
    {
        SaveManager.Instance.SaveGame();
    }
}