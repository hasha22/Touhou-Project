using UnityEngine;
namespace KH
{
    [CreateAssetMenu(menuName = "Movement/Patterns/Curved")]
    public class CurvedMovement : EnemyMovementPattern
    {
        public float horizontalSpeed = 2f;
        public float verticalSpeed = 1f;
        public float curveFrequency = 2f;
        public override Vector2 GetNextPosition(Transform enemyTransform, float deltaTime, float timeInPattern)
        {
            float curve = Mathf.Sin(timeInPattern * curveFrequency) * horizontalSpeed;
            return new Vector2(curve, -verticalSpeed) * deltaTime;
        }
        /*
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

        public override Vector2 GetNextPosition(Transform enemyTransform, float deltaTime, float timeInPattern)
        {
            float dirY = moveDown ? -1f : 1f;
            float yMovement = dirY * moveSpeed * deltaTime;

            float xMovement = Mathf.Sin(Time.time * curveFrequency + timeOffset) * curveAmplitude * deltaTime * directionMultiplier;

            return new Vector2(xMovement, yMovement);
        }*/
    }
}

