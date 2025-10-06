using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Movement/Straight Vertical")]
    public class VerticalMovement : EnemyMovementPattern
    {
        public bool isMovingDown = true;

        public override Vector2 GetNextPosition(Transform enemyTransform, float deltaTime)
        {
            float direction = isMovingDown ? -1f : 1f;

            return new Vector2(0f, direction * moveSpeed * deltaTime);
        }
    }
}
