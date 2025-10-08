using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Movement/Patterns/Straight Vertical")]
    public class VerticalMovement : EnemyMovementPattern
    {
        public bool isMovingDown = true;

        public override Vector2 GetNextPosition(Transform enemyTransform, float deltaTime, float timeInPattern)
        {
            float vertical = isMovingDown ? -1f : 1f;
            float speed = moveSpeed * accelerationCurve.Evaluate(timeInPattern);

            Vector2 direction = new Vector2(0f, vertical).normalized;

            return direction * speed * deltaTime;
        }
    }
}
