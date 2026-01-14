using UnityEngine;

public class LoadTester : MonoBehaviour
{
    void Start()
    {
        SaveManager.Instance.LoadGame();
    }
}