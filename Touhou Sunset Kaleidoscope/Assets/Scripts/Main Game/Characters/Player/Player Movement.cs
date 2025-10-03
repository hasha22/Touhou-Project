using UnityEngine;
namespace KH
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        private PlayerManager playerManager;
        private Animator animator;
        private PlayerAnimator playerAnimator;

        [Header("Player Movement")]
        public float currentSpeed;
        private float verticalMovement;
        private float horizontalMovement;
        private Rigidbody2D rb;

        [Header("Player Bounds")]
        public Transform playableArea;
        private Vector2 minBounds, maxBounds;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerManager = GetComponent<PlayerManager>();
            animator = GetComponent<Animator>();
            playerAnimator = GetComponent<PlayerAnimator>();
        }
        private void Start()
        {
            BoxCollider2D area = playableArea.GetComponent<BoxCollider2D>();
            Bounds bounds = area.bounds;
            minBounds = bounds.min;
            maxBounds = bounds.max;
            currentSpeed = playerManager.characterData.flyingSpeed;
        }
        private void FixedUpdate()
        {
            HandleMovement();
            playerAnimator.Animate(animator, horizontalMovement);
        }
        private void GetMovement()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
        }
        private void HandleMovement()
        {
            GetMovement();

            // Determining flying speed value
            float targetSpeed = PlayerInputManager.instance.isInPrecision ? playerManager.characterData.precisionSpeed : playerManager.characterData.flyingSpeed;
            // Smooth speed change
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, playerManager.characterData.speedLerp * Time.fixedDeltaTime);

            Vector2 moveAmount = new Vector2(horizontalMovement, verticalMovement);
            Vector2 velocity = moveAmount * currentSpeed;

            // Predicts next position
            Vector2 nextPosition = rb.position + velocity * Time.fixedDeltaTime;

            // Cancels velocity if it would be out of bounds
            if (nextPosition.x < minBounds.x || nextPosition.x > maxBounds.x)
            {
                velocity.x = 0;
            }
            if (nextPosition.y < minBounds.y || nextPosition.y > maxBounds.y)
            {
                velocity.y = 0;
            }

            rb.linearVelocity = velocity;
        }
    }
}