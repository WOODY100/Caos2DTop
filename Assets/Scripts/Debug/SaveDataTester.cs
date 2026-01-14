using UnityEngine;

public class SaveDataTester : MonoBehaviour
{
    void Start()
    {
        SaveData data = new SaveData();
        data.sceneName = "TestScene";
        data.playerPosition = new Vector3(5, 2, 0);
        data.inventoryItemIDs.Add("Sword_Iron");
        data.coins = 10;

        string json = JsonUtility.ToJson(data, true);
        Debug.Log(json);
    }
}