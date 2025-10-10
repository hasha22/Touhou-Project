using UnityEngine;
namespace KH
{
    public class EnemyWall : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                ObjectPool.instance.ReturnToPool(other.gameObject);
            }
        }
    }
}

