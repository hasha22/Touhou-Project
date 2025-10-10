using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Movement/Patterns/Straight Vertical")]
    public class VerticalMovement : EnemyMovementPattern
    {
        public bool isMovingDown = true;

        public override Vector2 GetTotalMovement(Transform enemyTransform, float duration)
        {
            float vertical = isMovingDown ? -1f : 1f;
            float totalDistance = moveSpeed * duration;
            Vector2 direction = new Vector2(0f, vertical);
            return direction * totalDistance;
        }
    }
}
