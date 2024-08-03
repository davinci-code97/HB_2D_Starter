using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA, pointB;
    [SerializeField] private float speed;

    Vector3 target;

    void Start()
    {
        transform.position = pointA.position;
        target = pointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, pointA.position) < .1f ) {
            target = pointB.position;
        } else if (Vector2.Distance(transform.position, pointB.position) < .1f) {
            target = pointA.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.transform.SetParent(null);
        }
    }

}
