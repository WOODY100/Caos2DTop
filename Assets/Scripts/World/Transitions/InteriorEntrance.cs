using UnityEngine;
using UnityEngine.SceneManagement;

public class InteriorEntrance : MonoBehaviour
{
    [Header("Interior Type")]
    public InteriorType interiorType;

    [Header("Same Scene")]
    public Transform sameSceneTarget;

    [Header("New Scene")]
    public string sceneName;
    public string spawnID;

    [Header("Input")]
    public bool requireInput = true;
    public KeyCode key = KeyCode.E;

    bool playerInside;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    void Update()
    {
        if (!playerInside) return;

        if (!requireInput || Input.GetKeyDown(key))
            Enter();
    }

    void Enter()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (interiorType == InteriorType.SameScene)
        {
            player.transform.position = sameSceneTarget.position;
        }
        else
        {
            SpawnManager.Instance.SetSpawn(spawnID);
            SceneManager.LoadScene(sceneName);
        }
    }
}
