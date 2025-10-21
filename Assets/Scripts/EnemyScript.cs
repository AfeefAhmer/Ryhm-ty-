using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;          // Liikkumisnopeus
    public float moveDistance = 3f;   // Liikkeen pituus suunnassa
    public bool moveHorizontally = true; // Jos false, liikkuu pystysuunnassa

    private Vector3 startPos;
    private int direction = 1;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (moveHorizontally)
        {
            transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
            if (Mathf.Abs(transform.position.x - startPos.x) >= moveDistance)
                direction *= -1;
        }
        else
        {
            transform.Translate(Vector2.up * speed * direction * Time.deltaTime);
            if (Mathf.Abs(transform.position.y - startPos.y) >= moveDistance)
                direction *= -1;
        }
    }
}
