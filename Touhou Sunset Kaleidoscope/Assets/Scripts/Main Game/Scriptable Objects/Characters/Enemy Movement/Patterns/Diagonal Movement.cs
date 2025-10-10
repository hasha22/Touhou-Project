using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Movement/Patterns/Diagonal Movement")]
    public class DiagonalMovement : EnemyMovementPattern
    {
        public bool isMovingRight = true;
        public bool isMovingUp = true;
        public override Vector2 GetTotalMovement(Transform enemyTransform, float duration)
        {
            float horizontal = isMovingRight ? 1f : -1f;
            float vertical = isMovingUp ? 1f : -1f;

            float totalDistance = moveSpeed * duration;
            Vector2 direction = new Vector2(horizontal, vertical).normalized;
            return direction * totalDistance;
        }
    }
}
