using KH;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [Header("Bomb Setup")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float startRadius = 1f;
    [SerializeField] private float endRadius = 2f;
    [SerializeField] private int bombDamage = 50;
    private float timer;

    [Header("Visuals")]
    [SerializeField] private Transform visualTransform;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private AnimationCurve expansionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = visualTransform.GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        timer = 0f;
        circleCollider.radius = startRadius;
    }
    private void Update()
    {
        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / lifetime);

        float radius = Mathf.Lerp(startRadius, endRadius, expansionCurve.Evaluate(t));
        circleCollider.radius = radius;

        float spriteUnitDiameter = spriteRenderer.sprite.bounds.size.x;
        float desiredDiameter = radius * 2f;
        float childScale = desiredDiameter / spriteUnitDiameter;
        spriteRenderer.transform.localScale = Vector3.one * childScale;

        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        DamageEnemies(radius);

        if (timer >= lifetime)
            Destroy(gameObject);
    }
    private void DamageEnemies(float radius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyController enemyController = hit.GetComponent<EnemyController>();
                enemyController.TakeDamage(bombDamage);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy Bullet"))
        {
            GameObject bulletObject = collision.gameObject;
            ObjectPool.instance.ReturnToPool(bulletObject);
        }
    }
}
