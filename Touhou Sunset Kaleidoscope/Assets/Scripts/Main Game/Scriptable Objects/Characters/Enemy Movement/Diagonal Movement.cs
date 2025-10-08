using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Movement/Diagonal Movement")]
    public class DiagonalMovement : EnemyMovementPattern
    {
        public bool isMovingRight = true;
        public bool isMovingUp = true;
        public override Vector2 GetNextPosition(Transform enemyTransform, float deltaTime)
        {
            float horizontalDirection = isMovingRight ? 1f : -1f;
            float verticalDirection = isMovingUp ? 1f : -1f;

            Vector2 direction = new Vector2(horizontalDirection, verticalDirection).normalized;

            return direction * moveSpeed * deltaTime;
        }
    }
}
