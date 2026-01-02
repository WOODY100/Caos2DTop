using UnityEngine;

public class EntradaColina : MonoBehaviour
{
    public Collider2D[] hillColliders;
    public Collider2D[] boundaryColliders;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (Collider2D hill in hillColliders)
            {
                hill.enabled = false;
            }
            
            foreach (Collider2D boudary in boundaryColliders)
            {
                boudary.enabled = true;
            }

            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 15;
        }
    }
}
