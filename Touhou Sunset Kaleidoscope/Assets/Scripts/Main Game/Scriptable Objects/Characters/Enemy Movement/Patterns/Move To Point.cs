using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Movement/Patterns/Move To Point")]
    public class MoveToPoint : EnemyMovementPattern
    {
        [Header("Target World Position")]
        public Vector2 targetPosition = new Vector2(-2.5f, 3f);

        public override Vector2 GetTotalMovement(Transform enemyTransform, float duration)
        {
            // total movement = target position - current position
            return targetPosition - (Vector2)enemyTransform.position;
        }
    }
}
