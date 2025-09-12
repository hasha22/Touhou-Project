using KH;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }

    [Header("Player Data")]
    public PlayableCharacterData characterData;
    [SerializeField] private int currentPlayerLives;
    [SerializeField] private float currentPower;

    // Temporary, to be moved to Audio Manager
    [SerializeField] private AudioClip deathSFX;

    private void Start()
    {
        UIManager.instance.UpdatePowerUI(currentPower);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy Bullet"))
        {
            Die();
            ObjectPool.instance.ReturnToPool(collision.gameObject);
        }
    }
    public void AddPower(float power)
    {
        currentPower += power;
        UIManager.instance.UpdatePowerUI(currentPower);
    }
    private void Die()
    {
        AudioSource.PlayClipAtPoint(deathSFX, transform.position, 1.0f);
        gameObject.SetActive(false);
    }
}
