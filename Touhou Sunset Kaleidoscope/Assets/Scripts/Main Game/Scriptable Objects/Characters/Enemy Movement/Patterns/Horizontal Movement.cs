using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Movement/Patterns/Straight Horizontal")]
    public class HorizontalMovement : EnemyMovementPattern
    {
        public bool isMovingRight = true;

        public override Vector2 GetTotalMovement(Transform enemyTransform, float duration)
        {
            float horizontal = isMovingRight ? 1f : -1f;
            float totalDistance = moveSpeed * duration;
            Vector2 direction = new Vector2(horizontal, 0f);
            return direction * totalDistance;
        }
    }
}

