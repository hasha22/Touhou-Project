using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Movement/Straight Horizontal")]
    public class HorizontalMovement : EnemyMovementPattern
    {
        public bool isMovingRight = true;

        public override Vector2 GetNextPosition(Transform enemyTransform, float deltaTime)
        {
            float direction = isMovingRight ? 1f : -1f;

            return new Vector2(direction * moveSpeed * deltaTime, 0f);
        }
    }
}

