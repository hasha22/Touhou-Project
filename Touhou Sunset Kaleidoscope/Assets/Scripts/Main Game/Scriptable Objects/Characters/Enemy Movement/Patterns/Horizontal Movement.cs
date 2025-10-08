using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Movement/Patterns/Straight Horizontal")]
    public class HorizontalMovement : EnemyMovementPattern
    {
        public bool isMovingRight = true;

        public override Vector2 GetNextPosition(Transform enemyTransform, float deltaTime, float timeInPattern)
        {
            float horizontal = isMovingRight ? 1f : -1f;
            float speed = moveSpeed * accelerationCurve.Evaluate(timeInPattern);

            Vector2 direction = new Vector2(horizontal, 0f).normalized;

            return direction * speed * deltaTime;
        }
    }
}

