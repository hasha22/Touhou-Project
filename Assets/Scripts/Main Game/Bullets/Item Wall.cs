using UnityEngine;
namespace KH
{
    public class ItemWall : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Score") ||
                other.CompareTag("Power") ||
                other.CompareTag("Faith") ||
                other.CompareTag("1 Up"))
            {
                ObjectPool.instance.ReturnToPool(other.gameObject);
            }
        }
    }
}
