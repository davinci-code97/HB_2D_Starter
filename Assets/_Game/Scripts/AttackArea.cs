using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] private GameObject hitVFX;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy")) {
            collision.GetComponent<Character>().OnHit(30f);
            GameObject hitVFXinstance = Instantiate(hitVFX, collision.transform.position, collision.transform.rotation);
            Destroy(hitVFXinstance, 1f);
        }   
    }

}
