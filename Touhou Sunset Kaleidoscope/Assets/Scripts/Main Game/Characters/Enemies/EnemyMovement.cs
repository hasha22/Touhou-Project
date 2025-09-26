using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private bool moveRight;

    private void Start()
    {
        moveSpeed = 2f;
        moveRight = true;
    }

    private void Update()
    {
        if (transform.position.x > 7f)
        {
            moveRight = false;
        }
        else if (transform.position.x < -7f)
        {
            moveRight = true;
        }

        if (moveRight)
        {
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
        }
    }
}
