using UnityEngine;
namespace KH
{
    public class Wall : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Bullet"))
            {
                ObjectPool.instance.ReturnToPool(other.gameObject);
            }
        }
    }
}

