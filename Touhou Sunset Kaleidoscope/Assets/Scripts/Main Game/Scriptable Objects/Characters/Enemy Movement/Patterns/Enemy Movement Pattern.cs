using UnityEngine;
namespace KH
{
    public abstract class EnemyMovementPattern : ScriptableObject
    {
        [Header("Move Speed")]
        public float moveSpeed = 2f;
        public virtual void Initialize(Transform enemyTransform)
        {

        }
        public abstract Vector2 GetTotalMovement(Transform enemyTransform, float duration); // gets its body from derived classes
    }
}
