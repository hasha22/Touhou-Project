using KH;
using UnityEngine;
[CreateAssetMenu(menuName = "Movement/Patterns/Pause")]
public class PauseMovement : EnemyMovementPattern
{
    public override Vector2 GetNextPosition(Transform enemyTransform, float deltaTime, float timeInPattern)
    {
        return Vector2.zero;
    }
}
