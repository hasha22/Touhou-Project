using KH;
using UnityEngine;
[CreateAssetMenu(menuName = "Movement/Patterns/Pause")]
public class PauseMovement : EnemyMovementPattern
{
    public override Vector2 GetTotalMovement(Transform enemyTransform, float deltaTime)
    {
        return Vector2.zero;
    }
}
