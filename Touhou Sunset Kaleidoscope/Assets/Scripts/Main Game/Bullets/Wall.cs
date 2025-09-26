using UnityEngine;
namespace KH
{
    public class Wall : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player Bullet") ||
                other.CompareTag("Enemy Bullet") ||
                other.CompareTag("Score") ||
                other.CompareTag("Power"))
            {
                ObjectPool.instance.ReturnToPool(other.gameObject);
            }
        }
    }
}

