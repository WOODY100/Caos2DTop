using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;

    public FloatingTextUI floatingTextPrefab;
    public Canvas worldCanvas;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDamage(int amount, Vector3 position, bool isCritical = false)
    {
        Spawn(amount, position, Color.red);
    }

    public void ShowHeal(int amount, Vector3 position)
    {
        Spawn(amount, position, Color.green);
    }

    private void Spawn(int amount, Vector3 worldPos, Color color)
    {
        FloatingTextUI text = Instantiate(
            floatingTextPrefab,
            worldPos,
            Quaternion.identity,
            worldCanvas.transform
        );

        text.Init(amount, color);
    }

    public void ShowExp(int amount, Vector3 position)
    {
        FloatingTextUI text = Instantiate(
            floatingTextPrefab,
            position,
            Quaternion.identity,
            worldCanvas.transform
        );

        text.InitExp(amount);
    }
}
