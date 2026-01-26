using UnityEngine;

public class ExitColinas : MonoBehaviour
{
    public Collider2D[] hillColliders;
    public Collider2D[] boundaryColliders;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (Collider2D hill in hillColliders)
            {
                hill.enabled = true;
            }

            foreach (Collider2D boudary in boundaryColliders)
            {
                boudary.enabled = false;
            }

            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
    }
}
