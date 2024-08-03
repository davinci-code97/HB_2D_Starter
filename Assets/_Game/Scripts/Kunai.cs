using UnityEngine;

public class Kunai : MonoBehaviour
{
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;

    void Start()
    {
        OnInit();
    }

    public void OnInit() {
        rb.velocity = transform.right * speed;
        Invoke(nameof(OnDespawn), 4f);
    }

    public void OnDespawn() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            collision.GetComponent<Character>().OnHit(30f);
            GameObject hitVFXinstance =  Instantiate(hitVFX, transform.position, transform.rotation);
            Destroy(hitVFXinstance, 1f);
            OnDespawn();
        }
    }

}
