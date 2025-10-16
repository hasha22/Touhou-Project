using UnityEngine;
namespace KH
{
    public class Wall : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player Bullet") ||
                other.CompareTag("Enemy Bullet") ||
                other.CompareTag("AfterImage"))
            {
                ObjectPool.instance.ReturnToPool(other.gameObject);
            }
        }
    }
}

