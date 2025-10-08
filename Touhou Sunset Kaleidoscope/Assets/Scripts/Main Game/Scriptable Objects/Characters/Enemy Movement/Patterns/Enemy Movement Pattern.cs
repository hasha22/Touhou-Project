using UnityEngine;
namespace KH
{
    public abstract class EnemyMovementPattern : ScriptableObject
    {
        [Header("Move Speed")]
        public float moveSpeed = 2f;
        public AnimationCurve accelerationCurve = AnimationCurve.Linear(0, 1, 1, 1);
        public virtual void Initialize(Transform enemyTransform)
        {

        }
        public abstract Vector2 GetNextPosition(Transform enemyTransform, float deltaTime, float timeInPattern);
    }
}
