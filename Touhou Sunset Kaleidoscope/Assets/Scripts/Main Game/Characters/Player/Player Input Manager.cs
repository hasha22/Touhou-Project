using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
namespace KH
{
    public class PlayerInputManager : MonoBehaviour
    {
        [HideInInspector] public InputControls inputControls;
        public static PlayerInputManager instance { get; private set; }

        private PlayerInput playerInput;

        [Header("Player Movement")]
        public Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount = 0;

        [Header("Player Actions")]
        public bool isShooting = false;
        public bool isInPrecision = false;
        public bool isBombing = false;

        [Header("Player Reference")]
        public GameObject playerObject;

        // PlayerInputManager stores a reference to the player game object, which can be accessed by other scripts when needed. This is done to save up on memory usage.
        // Serializing game objects decreases CPU usage, but increases memory usage, as opposed to using 
        // FindGameObjectWithTag which has O(n) complexity, and in a game with many game objects,
        // CPU performance is more critical.

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            if (playerObject == null)
            {
                Debug.LogError("Player Object is NULL!");
            }
            playerInput = GetComponent<PlayerInput>();
        }
        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChange;

            if (inputControls != null)
            {
                inputControls.Disable();
            }
        }
        private void Update()
        {
            HandleAllInputs();
        }
        private void HandleAllInputs()
        {
            HandleMovementInput();
        }

        private void OnEnable()
        {
            if (inputControls == null)
            {
                inputControls = new InputControls();
                inputControls.Enable();
            }
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }
        public void OnShoot(InputAction.CallbackContext context)
        {
            isShooting = 0 < context.ReadValue<float>();
        }
        public void OnPrecision(InputAction.CallbackContext context)
        {
            isInPrecision = 0 < context.ReadValue<float>();
        }
        public void OnBomb(InputAction.CallbackContext context)
        {
            isBombing = 0 < context.ReadValue<float>();
        }
        public void DisableInput()
        {
            playerInput.enabled = false;
            movementInput = Vector2.zero;
            isShooting = false;
            isInPrecision = false;
            isBombing = false;
        }
        public void EnableInput()
        {
            playerInput.enabled = true;
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                inputControls.Enable();
            }
            else
            {
                inputControls.Disable();
            }
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            //Enables controls if loading into Game Scene, disables otherwise
            if (newScene.buildIndex == 1)
            {
                instance.enabled = true;
                if (inputControls != null)
                {
                    inputControls.Enable();
                }
            }
            else
            {
                instance.enabled = false;
                if (inputControls != null)
                {
                    inputControls.Disable();
                }
            }
        }

        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
        }
    }
}