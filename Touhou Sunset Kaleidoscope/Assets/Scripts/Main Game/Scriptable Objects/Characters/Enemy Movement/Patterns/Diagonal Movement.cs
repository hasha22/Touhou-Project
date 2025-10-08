using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Movement/Patterns/Diagonal Movement")]
    public class DiagonalMovement : EnemyMovementPattern
    {
        public bool isMovingRight = true;
        public bool isMovingUp = true;
        public override Vector2 GetNextPosition(Transform enemyTransform, float deltaTime, float timeInPattern)
        {
            float horizontal = isMovingRight ? 1f : -1f;
            float vertical = isMovingUp ? 1f : -1f;

            //float speed = moveSpeed * accelerationCurve.Evaluate(timeInPattern);

            Vector2 direction = new Vector2(horizontal, vertical).normalized;

            return direction * moveSpeed * deltaTime;
        }
    }
}
