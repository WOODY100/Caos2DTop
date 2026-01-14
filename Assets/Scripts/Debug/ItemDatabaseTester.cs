using UnityEngine;

public class ItemDatabaseTester : MonoBehaviour
{
    void Start()
    {
        ItemData sword = ItemDatabase.Instance.GetItem("Dragon_Sword");

        if (sword != null)
            Debug.Log("Item cargado: " + sword.itemName);
        else
            Debug.LogError("Item no encontrado");
    }
}
