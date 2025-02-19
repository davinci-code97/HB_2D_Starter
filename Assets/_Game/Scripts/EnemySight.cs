using UnityEngine;

public class EnemySight : MonoBehaviour
{

    [SerializeField] private Enemy enemy;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            enemy.SetTarget(collision.GetComponent<Character>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            enemy.SetTarget(null);
        }
    }

}
