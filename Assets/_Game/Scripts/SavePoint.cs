using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && transform.position.x > collision.GetComponent<Player>().SavePoint.x) {
            collision.GetComponent<Player>().NewSavePoint();
        }
    }   
}
