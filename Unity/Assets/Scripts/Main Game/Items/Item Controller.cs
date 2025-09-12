using UnityEngine;
public class ItemController : MonoBehaviour
{
    [Header("Item Speed")]
    [SerializeField] private float fallSpeed = 1f;
    [SerializeField] private float pullSpeed = 5f;
    [SerializeField] private float pullRadius = 1.5f;
    private bool canBePulled = false;

    [Header("References")]
    private Transform playerMagnet;
    private PlayerManager playerManager;

    [Header("Score")]
    [SerializeField] private float addedPowerScore = 0.5f;
    [SerializeField] private int addedScore = 500;

    private void Start()
    {
        playerMagnet = GameObject.FindWithTag("Player Magnet").transform;
        playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if (playerMagnet == null) return;

        float distance = Vector2.Distance(transform.position, playerMagnet.position);
        if (distance <= pullRadius) { canBePulled = true; }

        if (canBePulled)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerMagnet.position, pullSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Magnet"))
        {
            if (gameObject.CompareTag("Power"))
            {
                playerManager.AddPower(addedPowerScore);
                Destroy(gameObject);
            }
            if (gameObject.CompareTag("Score"))
            {
                ScoreManager.instance.AddScore(addedScore);
                Destroy(gameObject);
            }
        }
    }
}
