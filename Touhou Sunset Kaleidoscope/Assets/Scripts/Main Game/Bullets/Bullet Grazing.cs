using UnityEngine;
namespace KH
{
    public class BulletGrazing : MonoBehaviour
    {
        public bool hasBeenGrazed = false;
        private Rigidbody2D rb;
        public Collider2D grazeCollider;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        private void OnDisable()
        {
            hasBeenGrazed = false;
        }
        public void OnGrazed()
        {
            ScoreManager.instance.RegisterGraze();
        }
    }
}

