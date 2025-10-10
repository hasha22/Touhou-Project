using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Movement/Patterns/Curved")]
    public class CurvedMovement : EnemyMovementPattern
    {
        public bool moveDown = true;
        public float curveAmplitude = 1f;
        public float curveFrequency = 2f;
        private float timeOffset;
        private float directionMultiplier;

        public override void Initialize(Transform enemyTransform)
        {
            // Randomize curve direction for variety
            directionMultiplier = Random.value > 0.5f ? 1f : -1f;
            timeOffset = Random.Range(0f, 2f * Mathf.PI);
        }

        public override Vector2 GetTotalMovement(Transform enemyTransform, float duration)
        {
            float dirY = moveDown ? -1f : 1f;

            // Total vertical movement
            float totalY = dirY * moveSpeed * duration;

            // For horizontal: integrate the sine wave over the duration
            // The integral of sin(x) is -cos(x)
            float endPhase = curveFrequency * duration + timeOffset;
            float startPhase = timeOffset;

            float totalX = (Mathf.Cos(startPhase) - Mathf.Cos(endPhase)) *
                           (curveAmplitude / curveFrequency) * directionMultiplier;

            return new Vector2(totalX, totalY);
        }
    }
}

